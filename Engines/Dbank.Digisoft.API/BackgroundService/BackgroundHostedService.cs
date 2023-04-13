using Dbank.Digisoft.Api.Abstractions;
using Dbank.Digisoft.Api.Data;
using Dbank.Digisoft.Ussd.Data.Extensions;
using Dbank.Digisoft.Ussd.Data.Models;
using Dbank.Digisoft.Ussd.SDK.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Api.BackgroundService
{
    public class BackgroundHostedService : IHostedService
    {
        private AppSettings _appSettings;
        private readonly ILogger<BackgroundHostedService> _logger;
        private readonly CancellationTokenSource _shutdown = new();
        private readonly ISmsClient _smsClient;
        private readonly IApplicationDataHelper _appHelper;
        private Task _backgroundTask = null!;

        public BackgroundHostedService(
            ILogger<BackgroundHostedService> loggerFactory,
            IOptionsMonitor<AppSettings> appSettingsOptions,
            ISmsClient smsClient, IServiceProvider serviceProvider)
        {
            _smsClient = smsClient;
            _logger = loggerFactory;
            _appSettings = appSettingsOptions.CurrentValue;
            appSettingsOptions.OnChange(appSettings => { _appSettings = appSettings; });
            _appHelper = null;//serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ChurchDataHelper>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service is starting");
            _backgroundTask = Task.Run(BackgroundProcessing, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service is stopping");
            _shutdown.Cancel();

            if (_backgroundTask != null)
                return Task.WhenAny(_backgroundTask, Task.Delay(Timeout.Infinite, cancellationToken));
            return Task.CompletedTask;
        }

        private async Task BackgroundProcessing()
        {
            while (!_shutdown.IsCancellationRequested)
            {
                try
                {
                    var responseList = new List<SmsOutbox>();
                    var request = await _appHelper.GetSmsOutboxByBatch(_appSettings.SmsBatchCount);
                    await request.ForEachAsync(async r =>
                    {
                        var response = await _smsClient.SendSms(r);
                        if (response.ResponseCode == ResponseCode.SUCCESS)
                        {
                            r.DateSent = DateTime.Now;
                            r.IsProcessing = false;
                            responseList.Add(r);
                        }
                    });

                    if (responseList.Count > 0)
                        await responseList.ForEachAsync(async r =>
                        {
                            await _appHelper.UpdateSmsOutboxById(r);
                        });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred processing {Method} in {Class}", 
                        nameof(BackgroundProcessing), nameof(BackgroundHostedService));
                }

                Thread.Sleep(TimeSpan.FromSeconds(_appSettings.WaitSeconds));
            }
        }
    }
}