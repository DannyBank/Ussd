using System.Collections.Generic;
using System.Linq;

// ReSharper disable once MemberCanBePrivate.Global

namespace Dbank.Digisoft.Ussd.SDK.Models;

public class AppOptions
{
    public string Agent { get; set; }
    public List<int> AllowedSC { get; set; }
    public string AppId { get; set; }
    public string BackPageCharacter { get; set; } = "0";
    public int BalanceCheckId { get; set; }
    public Dictionary<string, string> BalanceFields { get; set; } = new();
    public int BalancePosition { get; set; }
    public List<string> BarredNumbers { get; set; } = new();
    public List<int> BarredSC { get; set; } = new();
    public int BuyForOtherPosition { get; set; }
    public int BuyForOthersId { get; set; }
    public bool CanGotoPreviousMenu { get; set; }
    public string ConfirmationKey { get; set; }
    public string ConfirmCancelKey { get; set; }
    public string Credential { get; set; }
    public string DataDAType { get; set; }
    public int DeactivateId { get; set; }
    public int DeactivatePosition { get; set; }
    public bool DirectDialEnable { get; set; }
    public string ExtraDataSeparator { get; set; } = "~";
    public bool IncludeBalanceCheck { get; set; }
    public bool IncludeBuyForOthers { get; set; }
    public bool IncludeDeactivate { get; set; }
    public bool IncludeTransfer { get; set; }
    public bool IsTesting { get; set; }
    public string MenuSeparator { get; set; } = ")";
    public string NextPageCharacter { get; set; } = "*";
    public bool OffMenuDirectDialEnabled { get; set; }
    public string OffNetDAType { get; set; }
    public string OnNetDAType { get; set; }
    public string Origin { get; set; }
    public string OriginType { get; set; } = "UGW";
    public string ShortCode { get; set; }
    public string SmsDAType { get; set; }
    public string SMSSender { get; set; }
    public Dictionary<string, string> SmsSenders { get; set; } = new();
    public string MomoDebitReceiverAccount { get; set; }
    public decimal Rate { get; set; }
    public List<int> StagePageSizes { get; set; } = new();
    public List<string> Testers { get; set; } = new();
    public int TransferId { get; set; }
    public int TransferPosition { get; set; }
    public bool UseMultiPaymentChannels { get; set; }
    public bool UseParentCategories { get; set; }
    public List<bool> UseExtraMenus { get; set; } = new();
    public List<string> ShortCodes { get; set; } = new();
    public bool DelayPaymentRequest { get; set; }
    public int PaymentRequestDelay { get; set; }

    public int MaxPageAtStage(int stage)
    {
        return StagePageSizes.Count > stage
            ? StagePageSizes.ElementAtOrDefault(stage)
            : StagePageSizes.LastOrDefault();
    }
    public bool UseExtraMenu(int stage)
    {
        return UseExtraMenus.Count > stage
            ? UseExtraMenus.ElementAtOrDefault(stage)
            : UseExtraMenus.LastOrDefault();
    }

    public bool IsValidTester(string msisdn)
    {
        if (!IsTesting) return true;
        return Testers.Any() && Testers.Contains(msisdn);
    }
}