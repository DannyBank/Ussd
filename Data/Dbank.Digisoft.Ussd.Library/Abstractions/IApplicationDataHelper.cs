using Dbank.Digisoft.Ussd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Ussd.SDK.Abstractions {
    public interface IApplicationDataHelper {
        Task<List<SmsOutbox>?> GetSmsOutboxByBatch(int smsBatchCount);
        Task<SmsOutbox?> SaveSmsOutbox(SmsOutbox request);
        Task UpdateSmsOutboxById(SmsOutbox r);
    }
}
