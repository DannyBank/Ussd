namespace Dbank.Digisoft.Ussd.Data.Models {
    public class PaymentRequestModel {
        public string Msisdn { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ClientReference { get; set; } = string.Empty;
    }
}
