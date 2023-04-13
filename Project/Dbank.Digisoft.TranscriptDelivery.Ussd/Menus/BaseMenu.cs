namespace Dbank.Digisoft.TranscriptDelivery.Ussd.Menus {
    public class BaseMenu
    {
        
        public string Text { get; set; }
        public string Handler { get; set; } = "Index";
        public int Position { get; set; }
        public bool Active { get; set; }
        public string NextMenuType 
        {
            get  
            {
                return string.IsNullOrEmpty(_nextMenuType) ? null :
                    $"GreatIdeas.Ussd.Menus.{_nextMenuType}"; 
            }
            set { _nextMenuType = value; }
        }
        private string _nextMenuType;
    }
    public class MainMenu: BaseMenu { }
    public class OrderWithPaymentMenu : BaseMenu {
        public long ProductId { get; set; }
    }
    public class OrderWithoutPaymentMenu : BaseMenu {
        public long ProductId { get; set; }
    }
    public class DeactivationMenu : BaseMenu { }
    public class ConfirmProductMenu : BaseMenu
    {
        public int ProductId { get; set; }
        public string ConfirmMessage { get; set; }
    }
    public class PaymentChannelMenu : BaseMenu { }
    public class CurrencyOptionMenu : BaseMenu { }
    public class DirectDialMenu : BaseMenu {
        public List<int> ProductCodes { get; set; }
    }
    public class ConfirmPurchaseMenu : BaseMenu {}
    public class PredictionMenu : BaseMenu {
        public long BookingId { get; set; }
        public string PredictionValue { get; set; }
    }
    public class ViewPayMenu : BaseMenu {
        public long BookingId { get; set; }
        public string BookingCode { get; set; }
        public string Prediction { get; set; }
    }
    public class StartMenu : BaseMenu {}

}
