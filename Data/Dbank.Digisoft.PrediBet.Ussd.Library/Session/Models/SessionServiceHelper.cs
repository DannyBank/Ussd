using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Models
{
    public class SessionServiceHelper : ISessionServiceHelper
    {
        private readonly ILogger<SessionServiceHelper> _logger;
        private readonly HttpClient _httpClient;

        public SessionServiceHelper(ILogger<SessionServiceHelper> logger, IConfiguration configuration, HttpClient httpClient)

        {
            var baseUrl = configuration.GetConnectionString("Session");
            httpClient.BaseAddress = new Uri(baseUrl, UriKind.Absolute);
            _httpClient = httpClient;

            _logger = logger;
            AppId = configuration.GetValue<string>("App:AppId");
        }

        private readonly string AppId;

        public async Task<SessionInfo?> GetSession(long sessionId, string appId = "")
        {
            try
            {
                appId = string.IsNullOrWhiteSpace(appId) ? AppId : appId;
                var httpOut = await _httpClient.GetStringAsync(new Uri($"session/get/{appId}/{sessionId}", UriKind.Relative));
                var session = JsonConvert.DeserializeObject<SessionInfo>(httpOut);
                return session;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Occured Running {MethodName} for {@Data}", nameof(GetSession), new {sessionId, appId});
                return null;
            }
        }

        public async Task<SessionInfo?> GetSession(string msisdn, string dialogid, string appId = "")
        {
            try
            {
                appId = string.IsNullOrWhiteSpace(appId) ? AppId : appId;
                var httpOut = await _httpClient.GetStringAsync(new Uri($"session/get/{appId}/{msisdn}/{dialogid}", UriKind.Relative));
                var session = JsonConvert.DeserializeObject<SessionInfo>(httpOut);
                return session;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Occured Running {MethodName} for {@Data}", nameof(GetSession), new {msisdn, appId, dialogid});
                return null;
            }
        }

        public async Task<SessionInfo?> CreateSession(string msisdn, string dialogId, string appId = "")
        {
            try
            {
                appId = string.IsNullOrWhiteSpace(appId) ? AppId : appId;
                var sessionData = new SessionInfo(msisdn, dialogId)
                {
                    AppId = appId
                };
                var content = new StringContent(JsonConvert.SerializeObject(sessionData), Encoding.UTF8, "application/json");
                var httpOut = await _httpClient.PostAsync(new Uri($"session/create", UriKind.Relative), content);
                var session = JsonConvert.DeserializeObject<SessionInfo>(await httpOut.Content.ReadAsStringAsync());
                return session;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Occured Running {MethodName} for {@Data}", nameof(CreateSession), new {msisdn, dialogId, appId});

                return null;
            }
        }


        public SessionData? StoreSessionData(SessionInfo session, string dataKey, object? dataObject)
        {
            if (dataObject is null) throw new ArgumentNullException(nameof(dataObject));
            try
            {
                var output = new SessionData(session.SessionId, dataKey, dataObject.GetType(), JsonConvert.SerializeObject(dataObject));
                if (session.SessionsMeta is not null)
                {
                    var index = session.SessionsMeta.FindIndex(c => c.DataKey == output.DataKey);
                    if (index >= 0 )
                        session.SessionsMeta[index] = output;
                    else
                        session.SessionsMeta.Add(output);
                }
                else
                    session.SessionsMeta = new List<SessionData>() {output};

                return output;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Occured Running {MethodName} for {@Data}", nameof(StoreSessionData), new {session, dataKey, dataObject});
                return null;
            }
        }

        public async Task<SessionInfo?> CloseSession(SessionInfo sessionInfo)
        {
            try
            {
                await Update(sessionInfo);
                var httpOut = await _httpClient.GetStringAsync(new Uri($"session/end/{sessionInfo.Msisdn}/{sessionInfo.DialogId}/{sessionInfo.AppId}", UriKind.Relative));
                var session = JsonConvert.DeserializeObject<SessionInfo>(httpOut);
                return session;
            }
            catch (Exception e)
            {
                
                _logger.LogError(e, "Error Occured Running {MethodName} for {Data}", nameof(CloseSession), new {sessionInfo});

                return null;
            }
        }

        [Obsolete("Use SessionInfo.GetSessionData")]
        public async Task<T> GetSessionData<T>(long sessionId, string dataKey, string appId)
        {
            try
            {
                appId = string.IsNullOrWhiteSpace(appId) ? AppId : appId;
                var httpOut = await _httpClient.GetStringAsync(new Uri($"session/getsessiondata/{sessionId}/{dataKey}/{appId}", UriKind.Relative));
                var sessiondata = JsonConvert.DeserializeObject<T>(httpOut);
                return sessiondata;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Occured Running {MethodName} for {@Data}", nameof(GetSessionData), new {sessionId, dataKey, appId});
                throw;
            }
        }

        public async Task<SessionInfo?> Update(SessionInfo sessionInfo)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(sessionInfo), Encoding.UTF8, "application/json");
                var httpOut = await _httpClient.PostAsync(new Uri("session/update", UriKind.Relative), content);
                sessionInfo = JsonConvert.DeserializeObject<SessionInfo>(await httpOut.Content.ReadAsStringAsync());
                return sessionInfo;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Occured Running {MethodName} for {@Data}", nameof(Update), new {Session = sessionInfo});
                return null;
            }
        }

        public async Task<SessionInfo?> MovetoNextStageAsync(SessionInfo sessionInfo)
        {
            if (sessionInfo is null) throw new ArgumentNullException(nameof(sessionInfo));
            sessionInfo.Page = 1;
            sessionInfo.Stage++;
            return await Update(sessionInfo);
        }

        public async Task<SessionInfo?> MovetoNextPageAsync(SessionInfo sessionInfo)
        {
            if (sessionInfo is null) throw new ArgumentNullException(nameof(sessionInfo));
            sessionInfo.Page++;
            return await Update(sessionInfo);
        }

        public async Task<SessionInfo?> MovetoPrevPageAsync(SessionInfo sessionInfo)
        {
            if (sessionInfo is null) throw new ArgumentNullException(nameof(sessionInfo));
            if (sessionInfo.Page != 1)
                sessionInfo.Page--;
            return await Update(sessionInfo);
        }

        /*public async Task<SessionInfo?> CloseSession(string msisdn, string dialogId, string appId = "")
        {
            try
            {
                appId = string.IsNullOrWhiteSpace(appId) ? AppId : appId;
                var httpOut = await _httpClient.GetStringAsync(new Uri($"session/end/{msisdn}/{dialogId}/{appId}", UriKind.Relative));
                var session = JsonConvert.DeserializeObject<SessionInfo>(httpOut);
                return session;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Occured Running {MethodName} for {@Data}", nameof(CloseSession), new {msisdn, dialogId, appId});

                return null;
            }
        }*/

        [Obsolete("Data Retrieved True Get Session")]
        public async Task<string?> GetSessionData(long sessionId, string dataKey, string appId = "")
        {
            try
            {
                appId = string.IsNullOrWhiteSpace(appId) ? AppId : appId;
                var httpOut = await _httpClient.GetStringAsync(new Uri($"session/getsessiondata/{sessionId}/{dataKey}/{appId}", UriKind.Relative));
                var session = httpOut;
                return session;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Occured Running {MethodName} for {@Data}", nameof(GetSessionData), new {sessionId, dataKey, appId});

                return null;
            }
        }
    }
}