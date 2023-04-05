using System;

namespace Dbank.Digisoft.Ussd.Data.Models
{
    public class SmsOutbox
    {
        public long SmsId { get; set; }
        public string Recipient { get; set; }
        public string Origin { get; set; }
        public string Message { get; set; }
        public bool IsProcessing { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateSent { get; set; }
        public long OrderId { get; set; }
    }
}
