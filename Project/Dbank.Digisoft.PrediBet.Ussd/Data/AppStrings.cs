using Dbank.Digisoft.Ussd.Data.Models;

namespace Dbank.Digisoft.PrediBet.Ussd
{
    public class AppStrings:UssdMessages {
        public string CheckBalanceFormat { get; set; }
        public string DeactivationConfirmation { get; set; }
        public string BuyForOtherMsisdnPrompt { get; set; }
        public string InvalidMsisdnPassed { get; set; }
        public string NotAllowedMsisdnPassed { get; set; }
        public string LocationNotAllowed { get; set; }
        public string DeactivationCancelled { get; set; }
        public string ThankYouMessage { get; set; }
        public string PartyBSuccess { get; set; }
        public string PartyASuccess { get; set; }
        public string PartyASelf { get; set; }
        public string FailedMomo { get; set; }
        public string PurchaseFailed { get; set; }
        public string EnterProductCodePrompt { get; set; }
        public string EnterQuantityPrompt { get; set; }
        public string ConfirmPurchasePrompt { get; set; }
        public string NoActiveProducts { get; set; }
        public string NoPredictionsToday { get; set; }
        public string WelcomeMessage { get; set; }
        public string BookCodeEntryPrompt { get; set; }
        public string DisplayBookedSets { get; set; }
        public string InvalidBookingSelected { get; set; }
        public string EnterAmount { get; set; }
        public string ProcessPaymentPrompt { get; set; }
        public string InvalidPredictionSelected { get; set; }
        public string ChangeBookingPrompt { get; set; }
        public string NoBookingsForCode { get; set; }
        public string InvalidBookingCode { get; set; }
        public string ChangeBookingSuccessful { get; set; }
        public string SelectToUpdate { get; set; }
        public string SelectToViewOrPay { get; set; }

    }
}