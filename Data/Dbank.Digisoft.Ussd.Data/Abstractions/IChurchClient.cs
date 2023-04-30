using Dbank.Digisoft.Ussd.Data.Models;
using Dbank.Digisoft.Ussd.Data.Models.ChurchModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Ussd.Data.Abstractions
{
    public interface IChurchClient
    {
        Task<List<Church>?> GetAllChurches();
        Task<List<SubscriberChurchModel>?> GetChurchesBySubscriber(string msisdn);
        Task<Subscriber?> GetSubscriberByMsisdn(string msisdn);
        Task<List<Church>?> GetSubscribersByChurchId(long churchId);
        Task<Subscriber?> RecordSubscriber(Subscriber input);
        Task<SubscriberChurchModel?> RecordSubscriberForChurch(SubscriberChurchModel input);
    }
}