using System.Collections.Generic;
using System.Threading.Tasks;
using Dbank.Digisoft.Ussd.SDK.Session.Models;

namespace Dbank.Digisoft.Ussd.SDK.Session.Abstractions
{
    public interface ISessionDataHelper
    {
        Task ClearSessions();
        Task<(bool result, SessionInfo? data)> CreateSession(SessionInfo sessionInfo);
        Task<SessionInfo?> EndSession(long sessionId);
        Task<List<SessionInfo>> GetAbandonedSessions();
        Task<SessionData?> GetData(long sessionId, string key);
        Task<(bool result, SessionInfo? data)> GetSession(SessionInfo sessionInfo);
        Task<(bool result, SessionInfo? data)> UpdateSession(SessionInfo sessionInfo);
        Task<SessionData?> StoreData(SessionInfo sessionInfo, SessionData sesData);
        Task<List<SessionData>?> GetAllSessionData(long sessionId);
    }
}