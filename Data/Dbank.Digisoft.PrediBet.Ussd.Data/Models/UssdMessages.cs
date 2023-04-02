using System;
using System.Collections.Generic;
using System.Text;

namespace Dbank.Digisoft.PrediBet.Ussd.Data.Models
{
    public class UssdMessages
    {
        public string SuccesssUSDMessage { get; set; }
        public string BarredOutput { get; set; }
        public string TimeoutOutput { get; set; }
        public string UsedVoucherOutput { get; set; }
        public string ErrorOutput { get; set; }
        public string UnknownErrorOutput { get; set; }
        public string InvalidRequestFormatOutput { get; set; }
        public string InvalidVoucherOutput { get; set; }
        public string SuccessfulAirtimeTopOutput { get; set; }
        public string SelectProductResponsePreamble { get; set; }
        public string SelectProductPreamble { get; set; }
        public string ExtensionOutput { get; set; }
        public string InvalidInput { get; set; }
        public string InvalidProductSelected { get; set; }
        public string ExchangeRateFormat { get; set; }
        public string VoiceBalance { get; set; }
        public string DataBalance { get; set; }
        public string MaxReached { get; set; }
        public string BuyForOtherMaxReached { get; set; }
        public string ServiceClassNotAllowed { get; set; }
        public string AlreadyPurchased { get; set; }
        public string NotAllowed { get; set; } = "";
        public string SuccessTimeBound { get; set; }
        public string InsufficientBalance { get; set; }
        public string SuccessMomoMessage { get; set; }
        public string TimeDeactivateSuccess { get; set; }
        public string TimeDeactivateFailed { get; set; }
        public string TimedNotActive { get; set; }
        public string SuccessTimeBoundExtension { get; set; }
        public string CompanyNotActive { get; set; }
        public string MomoPaymentFailed { get; set; }
        public string DeactivateSuccess { get; set; }
        public string BuyForOtherName { get; set; }
        public string DeactivateName { get; set; }
        public string CheckBalanceName { get; set; }
        public string GenericError { get; set; } = "";
        public string BarredMsisdn { get; set; } = "You are not allowed to access this service. Please call 111 for more information.";
        public string UnknownShortCode { get; set; }
        public string NoActiveSubscription { get; set; }
        public string ConfirmationCancelled { get; set; }
        public string DeactivationFailed { get; set; }
        public string DeactivationSuccess { get; set; }
    }
}
