using Dbank.Digisoft.Api.Data;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Api.Abstractions
{
    public interface ISBClient {
        Task<BookingResponse?> GetBookingByCode(string bookingCode, long transId);
    }
}