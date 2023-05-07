namespace Dbank.Digisoft.Ussd.Data.Models.Hubtel {
    public class HubtelRequestModel {
        public decimal Amount { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ClientReference { get; set; } = string.Empty;
        public string CallbackUrl { get; set; } = string.Empty;
        public string CancellationUrl { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
    }
}
