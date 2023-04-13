namespace Dbank.Digisoft.Ussd.Data.Models.Hubtel {
    public class Data {
        public string PaylinkId { get; set; } = null!;
        public string ClientReference { get; set; } = null!;
        public string PaylinkUrl { get; set; } = null!;
        public int ExpiresAt { get; set; }
    }

    public class PaymentResponse {
        public string Message { get; set; } = null!;
        public string Code { get; set; } = null!;
        public Data Data { get; set; } = null!;
    }
}
