using System;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace Dbank.Digisoft.Ussd.SDK.Session.Models
{
    public class UssdMenuItem
    {
        public int Position { get; set; }
        public string? Text { get; set; }
        public Type? DataType { get; set; }
        public Type? DataConversionType { get; set; }
        public object? Data { get; set; }

        public void ProcessData()
        {
            if (Data is JObject)
                Data = JsonSerializer.Deserialize(Data?.ToString()??"", DataConversionType?? DataType);
        }
    }
}   