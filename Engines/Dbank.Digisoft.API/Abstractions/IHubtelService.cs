using Dbank.Digisoft.Api.Data;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Api.Abstractions
{
    public interface IHubtelService {
        Task<PaymentResponse> RequestPayment(PaymentRequest payload);
        Task<SmsResponse> SendSms(SmsRequest smsRequest);
    }
}