using System;
using Newtonsoft.Json;

namespace Dbank.Digisoft.Ussd.SDK.Session.Models
{
    public class SessionData
    {
        public long SessionId { get; set; }
        public string DataKey { get; set; }
        
        public string DataType { get; set; }
        public string DataValue { get; set; }
#pragma warning disable 8618
        public SessionData(){ }
#pragma warning restore 8618
        
        public SessionData(long sessionId, string dataKey, Type dataType, string valueString)
        {
            SessionId = sessionId;
            DataKey = dataKey;
            DataType = JsonConvert.SerializeObject(dataType);
            DataValue = valueString;
        }

        public object? GetValueObject(Type? T = null)
        {
            T ??= Type.GetType(DataType);
            return T switch
            {
                null => null,
                _ => JsonConvert.DeserializeObject(DataValue, T)
            };
        }
    }
}
