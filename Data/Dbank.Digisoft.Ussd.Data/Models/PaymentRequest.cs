using System;

namespace Dbank.Digisoft.Ussd.Data.Models
{
    public class PaymentRequest
    {
        public string Msisdn { get; set; } = null!;
        public string RequestTitle { get; set; } = null!; 
        public long RequestId { get; set; }
        public long TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ResponseDate { get; set; }
        public string Status { get; set; } = null!;
        public string RequestDescription { get; set; } = null!;
    }
}
