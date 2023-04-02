using Dbank.Digisoft.PrediBet.Ussd.Data.Models;

namespace Dbank.Digisoft.Church.Ussd
{
    public class AppStrings:UssdMessages
    {
        public string WelcomeMessage { get; set; }
        public string EnterAmount { get; set; }
        public string ProcessPaymentPrompt { get; set; }
    }
}