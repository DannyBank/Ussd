using Dbank.Digisoft.PrediBet.Api.Abstractions;
using Dbank.Digisoft.PrediBet.Api.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Api.Clients {
    public class SBClient : ISBClient {
        private readonly HttpClient _httpClient;

        public SBClient(HttpClient httpClient, IConfiguration configuration) 
        {
            httpClient.BaseAddress = new Uri(configuration.GetConnectionString("SB"), UriKind.Absolute);
            _httpClient = httpClient;
        }

        public async Task<BookingResponse?> GetBookingByCode(string bookingCode, long transId) 
        {
            if (string.IsNullOrWhiteSpace(bookingCode)) return null!;
            var httpOut =
                await _httpClient.GetStringAsync(
                    new Uri($"api/gh/orders/share/{bookingCode}?_t={transId}", UriKind.Relative));
            return JsonConvert.DeserializeObject<BookingResponse>(httpOut);
        }
    }
}
