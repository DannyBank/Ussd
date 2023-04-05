using Dbank.Digisoft.Ussd.Data.Models;

namespace Dbank.Digisoft.Ussd
{
    public class AppSettings:AppOptions
    {
        public double LRDRate { get; set; }
        public bool FreePurchase { get; set; }
        public bool AllowProductExtenstion { get; set; }
        public string AppCurrency { get; set; }
        public bool ConfirmationBeforePayment { get; set; }
        public string BalanceFilter { get; set; }
        public string BalanceExpiryField { get; set; }
        public bool CanGotoPreviousMenu { get; set; }
        public DaTypeFields DaTypeFields { get; set; }
        public bool OffMenuDirectDialEnabled { get;  set; }
    }

    public class DaTypeFields 
    {
        public string VoiceOnnet { get; set; }
        public string VoiceOffnet { get; set; }
        public string SMSOnnet { get; set; }
        public string SMSOffnet { get; set; }
        public string International { get; set; }
        public string Data { get; set; }
    }

}
