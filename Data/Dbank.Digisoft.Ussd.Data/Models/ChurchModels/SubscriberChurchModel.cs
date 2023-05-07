using System;

namespace Dbank.Digisoft.Ussd.Data.Models.ChurchModels
{
    public class SubscriberChurchModel
    {
        public int Id { get; set; }
        public string SubscriberName { get; set; }
        public string Msisdn { get; set; }
        public long ChurchId { get; set; }
        public string ChurchName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
