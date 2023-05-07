using Dbank.Digisoft.Ussd.SDK.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dbank.Digisoft.Ussd.SDK.Models;

public class UnoInput:UserInput
{
    [FromQuery(Name = "MSISDN")]
    public override string Msisdn { get; set; }
    [FromQuery(Name = "DIALOGUEID")]
    public override  string DialogId { get; set; }
    [FromQuery(Name = "TEXT")]
    public override string Input { get; set; }
    [FromQuery(Name = "USER_ID")]
    public string UserId { get; set; }
    [FromQuery(Name = "SERVICE_CODE")]
    public string ServiceCode { get; set; }
    [FromQuery(Name = "PASSWORD")]
    public string Password { get; set; }

}