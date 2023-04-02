using Dbank.Digisoft.PrediBet.Ussd.SDK.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Extensions;

public static class MvcExtensions
{
    public static UnoContent UnoResponse(this ControllerBase controller, string content, bool allowinput = false,
        string menu = "") => new(content, allowinput, menu);

    public static UnoContent UnoResponse(this ControllerBase controller, UssdMenu content) =>
        new(content.ToString(), content.ExpectInput);

    public static UssdContent UssdContent(this ControllerBase controller, string? content, bool allowInput = false,
        string menu = "") =>
        new(content, allowInput, menu);


    public static UssdContent UssdContent(this ControllerBase controller, UssdMenu content) =>
        new(content.ToString(), content.ExpectInput);
}