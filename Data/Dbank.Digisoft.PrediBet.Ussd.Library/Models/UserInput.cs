using Microsoft.AspNetCore.Mvc;

namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Models;

public class UserInput
{
    [FromQuery(Name = "msisdndigits")] public virtual string Msisdn { get; set; }
    [FromQuery(Name = "dialogueid")] public virtual string DialogId { get; set; }
    [FromQuery(Name = "ussdstring")] public virtual string Input { get; set; }
    [FromQuery(Name = "menu")] public string Menu { get; set; }
    [FromQuery(Name = "stage")] public string Stage { get; set; }
    [FromQuery(Name = "shortcode")] public string ShortCode { get; set; }
    [FromQuery(Name = "requesttype")] public string? RequestType { get; set; }
    [FromQuery(Name = "requestorigin")] public string? RequestOrigin { get; set; }
    [FromQuery(Name = "apitransactionId")] public string? RequestTransactionId { get; set; }


    //Location Details
    [FromQuery(Name = "location")] public string? Location { get; set; }
    [FromQuery(Name = "cellid")] public int? CellId { get; set; }
    [FromQuery(Name = "mcc")] public int? MCC { get; set; }
    [FromQuery(Name = "mnc")] public int? MNC { get; set; }
    [FromQuery(Name = "lac")] public int? LAC { get; set; }
    [FromQuery(Name = "isredirect")] public bool IsRedirect { get; set; }
}