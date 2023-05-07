using Dbank.Digisoft.PrediBet.Ussd.SDK.Models;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Extensions;

public static class UssdExtensions
{
    public static UssdMenu CreateUssdMenu(this MenuCollection menuItems) => menuItems.CreateUssdMenu(",");

    public static UssdMenu CreateUssdMenu(this MenuCollection menuitems, string seperator) =>
        menuitems.CreateUssdMenu(seperator, 1, AppConstants.DefaultMaxPage);

    public static UssdMenu CreateUssdMenu(this MenuCollection menuitems, string seperator, int currentPage,
        int maxPerPage)
    {
        var menuStrings = menuitems.MenuItems
            .OrderBy(c => c.Position)
            .Select(d => d.Text ?? string.Empty)
            .ToList();

        var ussdMenu = new UssdMenu(menuStrings.AsEnumerable(), menuitems.Count > maxPerPage * currentPage,
            currentPage > 1)
        {
            Header = menuitems.Header,
            Seperator = seperator,
            ExpectInput = menuitems.RequiresInput
        };

        return ussdMenu;
    }

    public static List<string> GetAllowedInputs(this MenuCollection menuitems, int maxPerPage,
        bool includeNext = false, bool includeBack = false, int page = 1, string allInputCharacter = "`")
    {
        var output = new List<string>();
        if (menuitems.RequiresInput)
        {
            if (menuitems.Count > 0)
            {
                output.AddRange(Enumerable.Range(1, menuitems.Count).Select(c => c.ToString()).ToList());
            }
            else
            {
                output.Add(allInputCharacter);
            }
        }

        return output;
    }
}