using Dbank.Digisoft.Ussd.Data.Models.PrediBet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Ussd.Data.Abstractions
{
    public interface IPrediBetClient
    {
        Task<Booking?> GetBookingById(long bookingId);
        Task<List<Booking>?> GetBookings();
        Task<List<Booking>?> GetBookingsByCode(string code);
    }
}