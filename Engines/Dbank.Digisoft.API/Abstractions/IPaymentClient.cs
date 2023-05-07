using Dbank.Digisoft.Api.Data;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Api.Abstractions
{
    public interface IPaymentClient
    {
        Task<PaymentResponse> RequestPayment(PaymentRequest payload);
    }
}