using Dbank.Digisoft.PrediBet.Api.Data;
using Dbank.Digisoft.PrediBet.Ussd.Data.Models;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Api.Abstractions
{
    public interface ISmsClient
    {
        Task<SmsResponse> SendSms(SmsOutbox request);
    }
}