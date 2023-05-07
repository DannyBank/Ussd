namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Models;

public class UssdMessages
{
    public string SuccessUSSDMessage { get; set; } = string.Empty;
    public string BarredOutput { get; set; } = string.Empty;
    public string TimeoutOutput { get; set; } = string.Empty;
    public string UsedVoucherOutput { get; set; } = string.Empty;
    public string ErrorOutput { get; set; } = string.Empty;
    public string UnknownErrorOutput { get; set; } = string.Empty;
    public string InvalidRequestFormatOutput { get; set; } = string.Empty;
    public string InvalidVoucherOutput { get; set; } = string.Empty;
    public string SuccessfulAirtimeTopOutput { get; set; } = string.Empty;
    public string SelectProductResponsePreamble { get; set; } = string.Empty;
    public string SelectProductPreamble { get; set; } = string.Empty;
    public string ExtensionOutput { get; set; } = string.Empty;
    public string InvalidInput { get; set; } = string.Empty;
    public string InvalidMsisdn { get; set; } = string.Empty;
    public string NotMtnNumber { get; set; } = string.Empty;
    public string InvalidProductSelected { get; set; } = string.Empty;
    public string ExchangeRateFormat { get; set; } = string.Empty;
    public string VoiceBalance { get; set; } = string.Empty;
    public string DataBalance { get; set; } = string.Empty;
    public string MaxReached { get; set; } = string.Empty;
    public string BuyForOtherMaxReached { get; set; } = string.Empty;
    public string ServiceClassNotAllowed { get; set; } = string.Empty;
    public string AlreadyPurchased { get; set; } = string.Empty;
    public string NotAllowed { get; set; } = "";
    public string SuccessTimeBound { get; set; } = string.Empty;
    public string InsufficientBalance { get; set; } = string.Empty;
    public string SuccessMomoMessage { get; set; } = string.Empty;
    public string TimeDeactivateSuccess { get; set; } = string.Empty;
    public string TimeDeactivateFailed { get; set; } = string.Empty;
    public string TimedNotActive { get; set; } = string.Empty;
    public string SuccessTimeBoundExtension { get; set; } = string.Empty;
    public string CompanyNotActive { get; set; } = string.Empty;
    public string MomoPaymentFailed { get; set; } = string.Empty;
    public string BuyForOtherName { get; set; } = string.Empty;
    public string DeactivateName { get; set; } = string.Empty;
    public string CheckBalanceName { get; set; } = string.Empty;
    public string GenericError { get; set; } = string.Empty;

    public string BarredMsisdn { get; set; } =
        "You are not allowed to access this service. Please call 100 for more information.";

    public string UnknownShortCode { get; set; } = string.Empty;
    public string NoActiveSubscription { get; set; } = string.Empty;
    public string ConfirmationCancelled { get; set; } = string.Empty;
    public string DeactivationFailed { get; set; } = string.Empty;
    public string DeactivationSuccess { get; set; } = string.Empty;
        
    public string FailedMomo { get; set; } = string.Empty;
    public string ThankYouMessage { get; set; } = string.Empty;

}