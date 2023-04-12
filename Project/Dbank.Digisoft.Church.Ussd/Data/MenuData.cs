using Dbank.Digisoft.Church.Ussd.Menus;
using System.Collections.Generic;

namespace Dbank.Digisoft.Church.Ussd {
    public class MenuData
    {
        public string SelectOptionHeader { get; set; }
        public string MainMenuHeader { get; set; }
        public string BuyForOtherHeader { get; set; }
        public List<MainMenu> MainMenu { get; set; }

        public string DeactivationHeader { get; set; }
        public List<DeactivationMenu> DeactivationOptionsMenu { get; set; }

        public string ConfirmProductHeader { get; set; }
        public List<ConfirmProductMenu> ConfirmProductMenu { get; set; }

        public string PaymentChannelsHeader { get; set; }
        public List<PaymentChannelMenu> PaymentChannelsMenu { get; set; }

        public string CurrencyOptionsHeader { get; set; }
        public List<CurrencyOptionMenu> CurrencyOptionsMenu { get; set; }
        public List<DirectDialMenu> DirectDialMenu { get; set; }
        public List<ConfirmPurchaseMenu> ConfirmPurchaseMenu { get; set; }
        public List<SubscriberMenu> SubscriberMenu { get; set; }
        public List<ChurchMenu> ChurchMenu { get; set; }
    }
}
