using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Models;

namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Abstractions
{
    public interface ISessionServiceHelper
    {
        Task<SessionInfo?> CloseSession(SessionInfo sessionInfo);
        [Obsolete("Use Model Version ")]
        // Task<SessionInfo?> CloseSession(string msisdn, string dialogId, string appId = "");
        Task<SessionInfo?> CreateSession(string msisdn, string dialogId, string appId = "");
        Task<SessionInfo?> GetSession(long sessionId, string appId = "");
        Task<SessionInfo?> GetSession(string msisdn, string dialogid, string appId = "");
        [Obsolete("use SessionInfo.GetsessionData<>")]
        Task<string?> GetSessionData(long sessionId, string datakey, string appId = "");
        [Obsolete("Use SessionInfo.GetSessionData")]
        Task<T> GetSessionData<T>(long sessionId, string dataKey, string appId);
        SessionData? StoreSessionData(SessionInfo session, string dataKey, object? dataObject);
        Task<SessionInfo?> Update(Models.SessionInfo sessionInfo);
        Task<SessionInfo?> MovetoNextStageAsync(SessionInfo sessionInfo);
        Task<SessionInfo?> MovetoNextPageAsync(SessionInfo sessionInfo);
        Task<SessionInfo?> MovetoPrevPageAsync(SessionInfo sessionInfo);
    }
}