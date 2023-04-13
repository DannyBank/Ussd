using Dbank.Digisoft.Ussd.Data.Models.PrediBet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Ussd.Data.Clients {
    public class PrediBetClient {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PrediBetClient> _logger;

        public PrediBetClient(HttpClient httpClient, IConfiguration configuration,
            ILogger<PrediBetClient> logger) {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration.GetConnectionString("Data"));
            _logger = logger;
        }

        public async Task<List<Booking>?> GetBookings() {
            try {
                var response = await _httpClient.GetAsync(new Uri("api/predibet/bookings", UriKind.Relative));
                if (!response.IsSuccessStatusCode) return null;
                var httpOut = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Booking>?>(httpOut);
            }
            catch (Exception ex){
                return null;
            }
        }

        public async Task<List<Booking>?> GetBookingsByCode(string code) {
            try {
                var response = await _httpClient.GetAsync(new Uri($"api/predibet/booking/code/{code}", UriKind.Relative));
                if (!response.IsSuccessStatusCode) return null;
                var httpOut = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Booking>?>(httpOut);
            }
            catch {
                return null;
            }
        }

        public async Task<Booking?> GetBookingById(long bookingId) {
            try {
                var response = await _httpClient.GetAsync(new Uri($"api/predibet/booking/id/{bookingId}", UriKind.Relative));
                if (!response.IsSuccessStatusCode) return null;
                var httpOut = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Booking?>(httpOut);
            }
            catch {
                return null;
            }
        }
    }
}
