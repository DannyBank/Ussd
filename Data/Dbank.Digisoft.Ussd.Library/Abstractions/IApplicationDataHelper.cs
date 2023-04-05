using Dbank.Digisoft.Ussd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Ussd.SDK.Abstractions {
    public interface IApplicationDataHelper {
        Task<List<Booking>?> GetBookingByCode(string text);
        Task<Booking?> GetBookingById(long bookingId);
        Task<List<Booking>?> GetBookings();
        Task<List<SmsOutbox>> GetSmsOutboxByBatch(int smsBatchCount);
        Task<SmsOutbox> SaveSmsOutbox(SmsOutbox request);
        Task UpdateSmsOutboxById(SmsOutbox r);
    }
}
