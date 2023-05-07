using Dbank.Digisoft.Api.Data;
using Dbank.Digisoft.Ussd.Data.Models;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Api.Abstractions
{
    public interface ISmsClient
    {
        Task<SmsResponse> SendSms(SmsOutbox request);
    }
}