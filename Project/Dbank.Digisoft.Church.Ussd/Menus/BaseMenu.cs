using System.Collections.Generic;

namespace Dbank.Digisoft.Church.Ussd.Menus
{
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
        public override string ToString() {
            return $"Text: {Text}\tHandler: {Handler}";
        }
    }
    public class MainMenu: BaseMenu {
        public Digisoft.Ussd.Data.Models.ChurchModels.Church SelectedChurch { get; set; }
    }
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
    public class SubscriberMenu : BaseMenu {}
    public class ChurchMenu : BaseMenu {}
}
