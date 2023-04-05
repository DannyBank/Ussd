namespace Dbank.Digisoft.Api.Data
{
    public class PaymentResponse
    {
        public string Message { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public PaymentData Data { get; set; } = new();
    }

    public class PaymentData
    {
        public string PaylinkId { get; set; } = string.Empty;
        public string ClientReference { get; set; } = string.Empty;
        public string PaylinkUrl { get; set; } = string.Empty;
        public int ExpiresAt { get; set; }
    }

    public class PaymentWebhookData: PaymentData
    {
        public string PaymentType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class PaymentWebhookResponse
    {
        public string Message { get; set; } = string.Empty;
        public string ResponseCode { get; set; } = string.Empty;
        public PaymentWebhookData Data { get; set; } = new();
    }
}