using Dbank.Digisoft.Ussd.Data.Models;
using Dbank.Digisoft.Ussd.Data.Models.ChurchModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Ussd.Data.Clients {
    public class ChurchClient {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ChurchClient> _logger;

        public ChurchClient(HttpClient httpClient, IConfiguration configuration,
            ILogger<ChurchClient> logger) {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration.GetConnectionString("Data"));
            _logger = logger;
        }

        public async Task<List<Church>?> GetAllChurches() {
            try {
                var response = await _httpClient.GetAsync(new Uri("api/church/all", UriKind.Relative));
                if (!response.IsSuccessStatusCode) return null;
                var httpOut = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Church>?>(httpOut);
            }
            catch (Exception ex){
                return null;
            }
        }

        public async Task<List<SubscriberChurchModel>?> GetChurchesBySubscriber(string msisdn) {
            try {
                var response = await _httpClient.GetAsync(new Uri($"api/church/all/by/subscriber/{msisdn}", UriKind.Relative));
                if (!response.IsSuccessStatusCode) return null;
                var httpOut = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<SubscriberChurchModel>?>(httpOut);
            }
            catch {
                return null;
            }
        }

        public async Task<List<Church>?> GetSubscribersByChurchId(long churchId) {
            try {
                var response = await _httpClient.GetAsync(new Uri($"api/church/subscribers/churchid/{churchId}", UriKind.Relative));
                if (!response.IsSuccessStatusCode) return null;
                var httpOut = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Church>?>(httpOut);
            }
            catch {
                return null;
            }
        }

        public async Task<Subscriber?> GetSubscriberByMsisdn(string msisdn) {
            try {
                var response = await _httpClient.GetAsync(new Uri($"api/church/subscriber/{msisdn}", UriKind.Relative));
                if (!response.IsSuccessStatusCode) return null;
                var httpOut = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Subscriber?>(httpOut);
            }
            catch {
                return null;
            }
        }

        public async Task<SubscriberChurchModel?> RecordSubscriberForChurch(SubscriberChurchModel input) {
            try {
                var content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8,
                "application/json");
                var httpOut = await _httpClient.PostAsync(new Uri("api/church/record", UriKind.Relative), content);
                if (!httpOut.IsSuccessStatusCode) return null;
                var result =
                    JsonConvert.DeserializeObject<SubscriberChurchModel?>(await httpOut.Content.ReadAsStringAsync());
                return result;
            }
            catch {
                return null;
            }
        }

        public async Task<Subscriber?> RecordSubscriber(Subscriber input) {
            try {
                var content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8,
                "application/json");
                var httpOut = await _httpClient.PostAsync(new Uri("api/church/record/subscriber", UriKind.Relative), content);
                if (!httpOut.IsSuccessStatusCode) return null;
                var result =
                    JsonConvert.DeserializeObject<Subscriber?>(await httpOut.Content.ReadAsStringAsync());
                return result;
            }
            catch {
                return null;
            }
        }
    }
}
