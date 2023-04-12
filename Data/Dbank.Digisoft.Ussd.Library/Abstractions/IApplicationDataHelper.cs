using Dbank.Digisoft.Ussd.Data.Models;
using Dbank.Digisoft.Ussd.Data.Models.ChurchModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Ussd.SDK.Abstractions
{
    public interface IApplicationDataHelper {
        Task<List<Church>?> GetChurches();
        Task<List<SubscriberChurchModel>?> GetChurchesBySubscriber(string msisdn);
        Task<List<SmsOutbox>?> GetSmsOutboxByBatch(int smsBatchCount);
        Task<Subscriber?> GetSubscriberByMsisdn(string msisdn);
        Task<List<Church>?> GetSubscribersByChurchId(int churchId);
        Task<SubscriberChurchModel?> RecordChurchSubscriber(SubscriberChurchModel subscriberModel);
        Task<Subscriber?> RecordSubscriber(Subscriber subscriber);
        Task<SmsOutbox?> SaveSmsOutbox(SmsOutbox request);
        Task UpdateSmsOutboxById(SmsOutbox r);
    }
}
