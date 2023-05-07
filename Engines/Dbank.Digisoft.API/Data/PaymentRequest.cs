namespace Dbank.Digisoft.Api.Data
{
    public class PaymentRequest
    {
        public int Amount { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ClientReference { get; set; } = string.Empty;
        public string CallbackUrl { get; set; } = string.Empty;
        public string CancellationUrl { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Msisdn { get; set; } = string.Empty;
    }
}