using Dapper;
using Dbank.Digisoft.PrediBet.Ussd.Data.Abstractions;
using Dbank.Digisoft.PrediBet.Ussd.Data.Extensions;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Models
{
    public class SessionDataHelper : ISessionDataHelper
    {
        private readonly ILogger<SessionDataHelper> _logger;
        private readonly IDbHelper _db;

        public SessionDataHelper(ILogger<SessionDataHelper> logger, IDbHelper dbHelper)
        {
            _db = dbHelper;
            _logger = logger;
        }

        public async Task<SessionInfo?> EndSession(long sessionId)
        {
            try
            {
                using var connection = _db.CreateConnection("Session");
                var session = await connection.QuerySingleOrDefaultAsync<SessionInfo>("CloseSession",
                    new
                    {
                        sessionid = sessionId
                    },
                    commandType: CommandType.StoredProcedure);

                return session;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Occured Closing Session with {SessionId}", sessionId);
                return null;
            }
        }


        public async Task<(bool result, SessionInfo data)> GetSession(SessionInfo sessionInfo)
        {
            try
            {
                using var connection = _db.CreateConnection("Session");
                if (!string.IsNullOrWhiteSpace(sessionInfo.Msisdn) && !string.IsNullOrWhiteSpace(sessionInfo.DialogId))
                {
                    sessionInfo = await connection.QuerySingleOrDefaultAsync<SessionInfo>("GetSession",
                        new
                        {
                            msisdn = sessionInfo.Msisdn,
                            dialogid = sessionInfo.DialogId,
                            appid = sessionInfo.AppId
                        },
                        commandType: CommandType.StoredProcedure);
                }
                else
                {
                    sessionInfo = await connection.QuerySingleOrDefaultAsync<SessionInfo>("GetSession",
                        new
                        {
                            sessionid = sessionInfo.SessionId,
                            appid = sessionInfo.AppId
                        },
                        commandType: CommandType.StoredProcedure);
                    ;
                }

                if (sessionInfo != null)
                    sessionInfo.IsActive = true;
                return (sessionInfo is not null, sessionInfo);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Occured Retrieving Session {@InputSession}", sessionInfo);
                return (false, null);
            }
        }

        public async Task<(bool result, SessionInfo? data)> CreateSession(SessionInfo sessionInfo)
        {
            try
            {
                using var connection = _db.CreateConnection("Session");
                if (string.IsNullOrWhiteSpace(sessionInfo?.Msisdn) || string.IsNullOrWhiteSpace(sessionInfo.DialogId) || string.IsNullOrWhiteSpace(sessionInfo.AppId))
                {
                    _logger.LogError(new InvalidOperationException(
                            $"Cannot create session without either {nameof(sessionInfo.Msisdn)} {nameof(sessionInfo.DialogId)}or {nameof(sessionInfo.AppId)}"),
                        "Error Occured Creating Session");
                    return (false, null);
                }

                if (sessionInfo.SessionId == default)
                    sessionInfo.SessionId = sessionInfo.Msisdn.GenerateTransactionId();
                sessionInfo = await connection.QuerySingleOrDefaultAsync<SessionInfo>("CreateSession",
                    new
                    {
                        msisdn = sessionInfo.Msisdn,
                        dialogid = sessionInfo.DialogId,
                        appid = sessionInfo.AppId,
                        sessionid = sessionInfo.SessionId
                    },
                    commandType: CommandType.StoredProcedure);
                if (sessionInfo != null)
                    sessionInfo.IsActive = true;
                return (sessionInfo != null, sessionInfo);
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Error Occured Creating Session {@Session}", sessionInfo);
                return (false, null);
            }
        }

        public async Task<(bool result, SessionInfo? data)> UpdateSession(SessionInfo? sessionInfo)
        {
            try
            {
                using var connection = _db.CreateConnection("Session");
                if (sessionInfo == null)
                {
                    _logger.LogError(new InvalidOperationException($"Cannot Update Null Session"), "Error Occured");
                    return (false, null);
                }

                sessionInfo = await connection.QuerySingleOrDefaultAsync<SessionInfo>("UpdateSession", new
                    {
                        sessionid = sessionInfo.SessionId,
                        stage = (int) sessionInfo.Stage,
                        page = sessionInfo.Page
                    },
                    commandType: CommandType.StoredProcedure);
                if (sessionInfo != null)
                    sessionInfo.IsActive = true;
                return (sessionInfo != null, sessionInfo);
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Error Occured Updating Session");
                return (false, null);
            }
        }

        public async Task<SessionData?> StoreData(SessionInfo? sessionInfo, SessionData sesData)
        {
            try
            {
                using var connection = _db.CreateConnection("Session");
                if (sessionInfo is null)
                {
                    _logger?.LogError(new InvalidOperationException(
                            $"Cannot create session without either {nameof(sessionInfo.Msisdn)} {nameof(sessionInfo.DialogId)}or {nameof(sessionInfo.AppId)}"),
                        "Error Occured Storing Session Data {@SessionData}", sesData);
                    return null;
                }

                if (sessionInfo.SessionId == 0)
                {
                    _logger?.LogError(new InvalidOperationException("Session has no SessionID"), "Error Occured");
                    return null;
                }

                var sessionData = await connection.QuerySingleOrDefaultAsync<SessionData>("SetSessionData",
                    new
                    {
                        sessionid = sessionInfo.SessionId,
                        datakey = sesData.DataKey,
                        datavalue = sesData.DataValue,
                        datatype = sesData.DataType
                    },
                    commandType: CommandType.StoredProcedure);
                return sessionData;
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Error Occured Storing Session Data {@SessionData}", sesData);
                return null;
            }
        }

        public async Task<SessionData?> GetData(long sessionId, string key)
        {
            try
            {
                using var connection = _db.CreateConnection("Session");
                if (sessionId == 0)
                {
                    _logger?.LogError(new InvalidOperationException("Session has no SessionID"),
                        "Error Occured Retrieving Session Data {SessionId},{DataKey}", sessionId,key);
                    return null;
                }
                var sessionData = await connection.QuerySingleOrDefaultAsync<SessionData>("GetSessionData", 
                    new
                    {
                        sessionid = sessionId,
                        datakey = key
                    },
                    commandType: CommandType.StoredProcedure);
                return sessionData;
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Error Occured Retrieving Session Data {SessionId},{DataKey}", sessionId,key);
                return null;
            }
        }

        public async Task<List<SessionData>?> GetAllSessionData(long sessionId)
        {
            try
            {
                using var connection = _db.CreateConnection("Session");
                if (sessionId == 0)
                {
                    _logger?.LogError(new InvalidOperationException("Session has no SessionID"),
                        "Error Occured Retrieving Session Data for {SessionId}", sessionId);
                    return null;
                }

                //TODo Use Dapper
                var sessionData = await connection.QueryAsync<SessionData>("GetAllSessionData", 
                    new
                    {
                        sessionid = sessionId
                    },
                    commandType: CommandType.StoredProcedure);
                ;
                return sessionData.ToList();
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Error Occured Retrieving Session Data for {SessionId}", sessionId);
                return null;
            }
        }

        public async Task ClearSessions()
        {
            using var connection = _db.CreateConnection("Session");
            await connection.ExecuteAsync("ClearSessions",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<SessionInfo>> GetAbandonedSessions()
        {
            using var connection = _db.CreateConnection();
            var sessions = await connection.QueryAsync<SessionInfo>("GetAbandonedSessions",
                commandType: CommandType.StoredProcedure);
            return sessions.ToList();
        }
    }
}