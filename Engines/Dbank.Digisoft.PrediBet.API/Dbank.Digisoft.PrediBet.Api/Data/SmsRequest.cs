namespace Dbank.Digisoft.PrediBet.Api.Data
{
    public class SmsRequest
    {
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}