using Dbank.Digisoft.Ussd.Data.Models;

namespace Dbank.Digisoft.Church.Ussd
{
    public class AppStrings:UssdMessages
    {
        public string WelcomeMessage { get; set; }
        public string EnterAmount { get; set; }
        public string ProcessPaymentPrompt { get; set; }
        public string EnterNamePrompt { get; set; }
        public string NoChurchWasSelected { get; set; }
        public string RecordChurchSubscriberFailed { get; set; }
        public string RecordChurchSubscriberSuccess { get; set; }
        public string WelcomeSubscriberToChurch { get; set; }
        public string SelectChurchPrompt { get; set; }
        public string SelectChurchAction { get; set; }
    }
}