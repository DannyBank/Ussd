using Dbank.Digisoft.PrediBet.Api.Data;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Api.Abstractions
{
    public interface ISBClient {
        Task<BookingResponse?> GetBookingByCode(string bookingCode, long transId);
    }
}