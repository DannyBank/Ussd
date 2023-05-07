using Dbank.Digisoft.Ussd.Data.Models;
using Dbank.Digisoft.Ussd.Data.Models.Hubtel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Ussd.SDK.Helper {
    public class HubtelHelper {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        private readonly string _hashed;
        private readonly string _callbackUrl;
        private readonly string _cancelUrl;
        private readonly string _returnUrl;

        public HubtelHelper(HttpClient httpClient, IConfiguration config) {
            _httpClient = httpClient;
            _config = config;
            _hashed = _config.GetConnectionString("Hubtel:Credentials");
            _callbackUrl = _config.GetConnectionString("Hubtel:CallBack");
            _cancelUrl = _config.GetConnectionString("Hubtel:Cancellation");
            _returnUrl = _config.GetConnectionString("Hubtel:Return");
        }

        public async Task<PaymentResponse?> RequestSingle(PaymentRequest model, string msisdn) {
            try {                
                var decrypted = _hashed;
                var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes(decrypted));
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64String);

                var input = new HubtelRequestModel {
                    Amount = model.Amount,
                    Title = model.RequestTitle,
                    Description = model.RequestDescription,
                    ClientReference = model.TransactionId.ToString(),
                    CallbackUrl = _callbackUrl,
                    CancellationUrl = _cancelUrl,
                    ReturnUrl = _returnUrl
                };
                var content = new StringContent(
                    JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
                var url = $"{_config.GetConnectionString("Hubtel:RequestSingle")}{msisdn}";
                var httpOut = await _httpClient.PostAsync(url, content);
                if (!httpOut.IsSuccessStatusCode) return null;
                var result =
                    JsonConvert.DeserializeObject<PaymentResponse>(await httpOut.Content.ReadAsStringAsync());
                return result ?? null;
            }
            catch {
                return null;
            }
        }
    }
}
