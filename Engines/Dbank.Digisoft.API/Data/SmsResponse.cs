namespace Dbank.Digisoft.Api.Data
{
    public class SmsResponse
    {
        public string Message { get; set; } = string.Empty;
        public string ResponseCode { get; set; } = string.Empty;
        public SmsData Data { get; set; } = new();
    }

    public class SmsData
    {
        public int Rate { get; set; }
        public string MessageId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string NetworkId { get; set; } = string.Empty;
    }
}