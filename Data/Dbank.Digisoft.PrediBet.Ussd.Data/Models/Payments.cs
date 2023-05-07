using System;

namespace Dbank.Digisoft.PrediBet.Ussd.Data.Models
{
    public class Payments
    {
        public long PaymentId { get; set; }
        public long OrderId { get; set; }
        public double AmountPaid { get; set; }
        public double Balance { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
