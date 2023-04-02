using Dbank.Digisoft.PrediBet.Api.Data;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Api.Abstractions
{
    public interface IHubtelService {
        Task<PaymentResponse> RequestPayment(PaymentRequest payload);
        Task<SmsResponse> SendSms(SmsRequest smsRequest);
    }
}