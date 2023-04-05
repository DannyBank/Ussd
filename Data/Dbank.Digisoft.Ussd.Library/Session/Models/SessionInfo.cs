using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dbank.Digisoft.Ussd.SDK.Session.Models
{
    public class SessionInfo
    {
        public SessionInfo()
        {}
        public SessionInfo(string msisdn, string dialogId)
        {
            Msisdn = msisdn;
            DialogId = dialogId;
        }

        public long SessionId { get; set; }
        public string Msisdn { get; set; }
        public string DialogId { get; set; }
        public string AppId { get; set; }
        public int Page { get; set; }
        public SessionStage Stage { get; set; }
        public SessionStatus Status { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime StartTime { get; set; }
        public string CurrentInput { get; set; }
        public List<SessionData>? SessionsMeta { get; set; } = new List<SessionData>();
        public MenuCollection Data { get; set; } 

        public bool IsClosed => Status == SessionStatus.Completed;

        public T GetSessionData<T>(string key)
        {
            if (SessionsMeta is null)
                SessionsMeta = new List<SessionData>();
            var data = SessionsMeta.SingleOrDefault(s => s.DataKey == key);
            if (data is null) return default!;
            var parsed = JsonConvert.DeserializeObject<T>(data.DataValue);
            return parsed;

        }

        public SessionData? StoreSessionData(string dataKey, object? dataObject)
        {
            if (dataObject is null) return null;
            try
            {
                var output = new SessionData(SessionId, dataKey, dataObject.GetType(), JsonConvert.SerializeObject(dataObject));
                if (SessionsMeta is not null)
                {
                    var copy = new HashSet<SessionData>(SessionsMeta);
                    var index = copy.FirstOrDefault(c => c.DataKey == output.DataKey);
                    if (index is not null)
                    {
                        index.DataType = output.DataType;
                        index.DataValue = output.DataValue;
                    }
                    else
                    {
                        copy.Add(output);
                    }

                    SessionsMeta = copy.ToList();
                }
                else
                    SessionsMeta = new() { output };

                return output;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}