using System;

namespace Dbank.Digisoft.Ussd.Data.Models {
    public class Booking {
        public long BookingId { get; set; }
        public string BookingName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Home { get; set; }
        public string Away { get; set; }
        public string Prediction { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Active { get; set; }
        public override string ToString() => $"{Home} vs {Away} - {Prediction}";
    }
}
