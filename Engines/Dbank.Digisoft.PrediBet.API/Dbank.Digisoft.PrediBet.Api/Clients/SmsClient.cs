using Dbank.Digisoft.PrediBet.Api.Abstractions;
using Dbank.Digisoft.PrediBet.Api.Data;
using Dbank.Digisoft.PrediBet.Ussd.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Api.Clients
{
    public class SmsClient : ISmsClient
    {
        private readonly ILogger<SmsClient> _logger;
        private readonly IHubtelService _hubtelService;

        public SmsClient(ILogger<SmsClient> logger, IHubtelService hubtelService)
        {
            _logger = logger;
            _hubtelService = hubtelService;
        }

        public async Task<SmsResponse> SendSms(SmsOutbox request)
        {
            try
            {
                return await _hubtelService.SendSms(new SmsRequest
                {
                    Content = request.Message,
                    From = request.Origin,
                    To = request.Recipient
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred processing {Method} in {Class}",
                    nameof(SendSms), nameof(SmsClient));
                return new();
            }
        }
    }
}
