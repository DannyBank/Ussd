using Dbank.Digisoft.Ussd.SDK.Extensions;
using Dbank.Digisoft.Ussd.SDK.Session.Models;
using System;

namespace Dbank.Digisoft.Ussd.SDK.Models;

public struct AppConstants
{
    public static readonly Func<SessionStage, string> MENUDATA = (SessionStage stage) => $"{stage.ToDescriptionString()}MENU";
    public const string SELECTED_MENUS = "Selections";
    public const string SELECTED_PAYMENT = "SelectedPayment";
    public const string TRANSFER_KEY = "isTransfer";
    public const string PAYMENTOPTIONS = "Payments";
    public const string CONFIRMATION = "Confirmation";
    public const string BUYFOROTHERS_SESSIONKEY = "IsBuyForOthers";
    public const string OTHERMSISDN_SESSIONKEY = "OtherMsisdn";
    public const string CANCEL_SESSIONKEY = "CancelMsisdn";
    public const string CANCEL_PRODUCTS = "CancelProducts";
    public const string MENUITEMS = "MenuItems";
    public const string CATEGORY_LIST = "CurrentCategoryList";
    public const string SELECTED_CATEGORY = "SelectedCategory";
    public const string PRODUCTLIST = "CurrentProductList";
    public const string SELECTED_PRODUCT = "SelectedProduct";
    public const string SELECTED_PROMO_PRODUCT = "SelectedPromoForCancel";
    public const string CANCEL_PROMO_PRODUCT = "CancelPromoProduct";
    public const string ALLOWEDINPUT = "AllowedInput";
    public const string MODE = "Mode";
    public const string BFO_HEADER_KEY = "BuyForOtherHeader";
    public const string NoHeader = "NoHeader";
    public const string TRANSFER_MSISDN = "TransferNumber";
    public const string TRANSFER_AMT = "TransferAmount";
    public const string TRANSFERABLE = "TransferableAmount";
    public const string ISBUYFOROTHER = "IsBuyForOther";

    public const int DefaultMaxPage = 5;

    public const string SELECTED_CURRENCY = "selectedcurrency";

    public const string INITREQUEST = "initrequest";

    public const string MOMO_PAYMENT_CHANNEL = "MomoPayChnl";
    public const string AIRTIME_PAYMENT_CHANNEL = "AirtimePayChnl";
    public const string PAYMENT_CHANNEL= "PaymentChannel";

    public const string ACTIVATION_TYPE= "ActivationType";
    public const string ACTIVATION_TYPE_ONE_OFF = "OneOff";
    public const string ACTIVATION_TYPE_AUTO_RENEWAL = "AutoRenewal";

    public const string OTHER_SERVICECLASS = "CurrentMsisdnServiceClass";

    public const string SELECTED_BOOKING_CODE = "selectedbookingcode";
    public const string SELECTED_BOOKING_SET = "changedbookings";
    public const string SELECTED_BOOKING_ID = "selectedbookingid";
    public const string BOOKING_SET_UPDATED = "isbookingsetupdated";
}