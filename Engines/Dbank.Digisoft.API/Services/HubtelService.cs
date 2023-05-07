using Dbank.Digisoft.Api.Abstractions;
using Dbank.Digisoft.Api.Data;
using Dbank.Digisoft.Ussd.Data.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Api.Services
{
    public class HubtelService : IHubtelService
    {
        private readonly AppSettings _appSettings;
        private readonly string _paymentApi;
        private readonly string _smsApi;
        private readonly ILogger<HubtelService> _logger;

        public HubtelService(IOptionsSnapshot<AppSettings> optionsSnapshot,
            IConfiguration config, ILogger<HubtelService> loger)
        {
            _appSettings = optionsSnapshot.Value;
            _paymentApi = config.GetConnectionString("PaymentApi");
            _smsApi = config.GetConnectionString("SmsApi");
            _logger = loger;
        }

        public async Task<PaymentResponse> RequestPayment(PaymentRequest payload)
        {
            PaymentResponse response = new();
            try
            {
                var username = _appSettings.Username;
                var password = _appSettings.Password;
                using var client = new HttpClient();
                var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64String);
                var json = JsonConvert.SerializeObject(payload);
                var postData = new StringContent(json, Encoding.UTF8, "application/json");
                var MobileNumber = payload.Msisdn.ToIntMsisdn(IntCode.GHA);
                var request = await client.PostAsync(_paymentApi + MobileNumber, postData);
                var content = await request.Content.ReadAsStringAsync();

                if (content != null)
                    response = JsonConvert.DeserializeObject<PaymentResponse>(content) ?? new();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred processing {Method} in {Class}",
                    nameof(RequestPayment), nameof(HubtelService));
                return response;
            }
        }

        public async Task<SmsResponse> SendSms(SmsRequest payload)
        {
            SmsResponse response = new();
            try
            {
                var username = _appSettings.Username;
                var password = _appSettings.Password;
                using var client = new HttpClient();
                var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64String);
                var json = JsonConvert.SerializeObject(payload);
                var postData = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                var request = await client.PostAsync(_smsApi, postData);
                var content = await request.Content.ReadAsStringAsync();

                if (content != null)
                    response = JsonConvert.DeserializeObject<SmsResponse>(content) ?? new();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred processing {Method} in {Class}",
                    nameof(SendSms), nameof(HubtelService));
                return response;
            }
        }
    }
}
