using Dbank.Digisoft.Ussd.Data.Models;
using Dbank.Digisoft.Ussd.Data.Models.Hubtel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Ussd.SDK.Helper {
    public class HubtelHelper {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public HubtelHelper(HttpClient httpClient, IConfiguration config) {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<PaymentResponse?> RequestSingle(PaymentRequest model, string msisdn) {
            try {
                var hashed = _config.GetSection("HubtelCredentials").Value;
                var decrypted = hashed;
                var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes(decrypted));
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64String);

                var input = new PaymentRequestModel {
                    Amount = model.Amount,
                    Title = model.RequestTitle,
                    ClientReference = model.TransactionId.ToString(),
                    Description = model.RequestDescription,
                    Msisdn = model.Msisdn
                };
                var content = new StringContent(
                    JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
                var url = $"{_config.GetConnectionString("Hubtel:RequestSingle")}{msisdn}";
                var httpOut = await _httpClient.PostAsync(new Uri(url), content);
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
