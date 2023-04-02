using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dbank.Digisoft.PrediBet.Engine.Session.Controllers
{
    [Route("[controller]")]
    public class SessionController : Controller
    {
        private readonly ILogger<SessionController> _logger;
        private readonly SessionDataHelper _dataHelper;

        public SessionController(ILogger<SessionController> logger, SessionDataHelper dataHelper)
        {
            _logger = logger;
            _dataHelper = dataHelper;
        }

        [HttpGet("get/{appId}/{sessionId:long}")]
        public async Task<SessionInfo?> GetSession(string appId, long sessionId)
        {
            try
            {
                var session = new SessionInfo()
                {
                    SessionId = sessionId,
                    AppId = appId
                };
            
                var (result, data) = await _dataHelper.GetSession(session);
                if (result)
                {
                    data.SessionsMeta = await _dataHelper.GetAllSessionData(session.SessionId) ?? new List<SessionData>();
                }

                return result ? data : null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Occured Getting Session");
                return null;
            }
        }

        [HttpGet("get/{appId}/{msisdn}/{dialogid}")]
        public async Task<SessionInfo?> GetSession(string appId, string msisdn, string dialogid)
        {
            try
            {

                var session = new SessionInfo()
                {
                    Msisdn = msisdn,
                    AppId = appId,
                    DialogId = dialogid
                };
                (bool result, SessionInfo data) = await _dataHelper.GetSession(session);

                if (result)
                {
                    data.SessionsMeta = await _dataHelper.GetAllSessionData(data.SessionId);
                }

                return result ? data : null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPost("create")]
        public async Task<SessionInfo?> CreateSession([FromBody] SessionInfo sessionInfo)
        {
            try
            {
                (bool result, SessionInfo data) = await _dataHelper.CreateSession(sessionInfo);

                return result ? data : null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpGet("end/{msisdn}/{dialogid}/{appId}")]
        public async Task<SessionInfo?> EndSession(string msisdn, string dialogid, string appId)
        {
            try
            {
                var (result, data) = await _dataHelper.GetSession(new SessionInfo(msisdn, dialogid) {AppId = appId});
                if (result)
                {
                    SessionInfo sessionInfo = await _dataHelper.EndSession(data.SessionId);
                    return sessionInfo;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpGet("getsessiondata/{sessionId:long}/{dataKey}/{appId}")]
        public async Task<string?> GetSessionData(long sessionId, string datakey)
        {
            try
            {
                SessionData session = await _dataHelper.GetData(sessionId, datakey);
                return session?.DataValue;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPost("update")]
        public async Task<SessionInfo?> UpdateSession([FromBody] SessionInfo sessionInfo)
        {
            try
            {

                (bool result, SessionInfo? data) = await _dataHelper.UpdateSession(sessionInfo);
                if (result)
                {
                    bool dataStored = true;
                    if (sessionInfo.SessionsMeta != null)
                        foreach (var sessionData in sessionInfo.SessionsMeta)
                        {
                            var output = await _dataHelper.StoreData(data, sessionData);
                            dataStored &= output is not null;
                        }
                    if(!dataStored)
                    result &= dataStored;
                }
                return result ? data : null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPost("{appId}/addData")]
        public async Task<SessionData?> AddSessionData([FromBody] SessionData sesData, string appId)
        {
            try
            {
                var session = new SessionInfo
                {
                    SessionId = sesData.SessionId,
                    AppId = appId
                };
                await _dataHelper.GetSession(session);
                var output = await _dataHelper.StoreData(session, sesData);
                return output;
            }
            catch (Exception e)
            {

                return null;
            }
        }
    }
}