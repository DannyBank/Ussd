using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Models;

public class UssdMenu
{
    private readonly bool _hasNextPage;
    private readonly bool _hasPrevPage;
    public bool ExpectInput { get; set; }
    public string? Seperator { get;set;} 
    public string? NextCharacter { get; set; } = null;
    public string? BackCharacter { get; set; } = null;

    public UssdMenu() { }

    public UssdMenu(IEnumerable<string?>? menuItems, bool hasNextPage = false, bool hasPrevPage = false, bool expectInput = false)
    {
        _hasNextPage = hasNextPage;
        _hasPrevPage = hasPrevPage;
        var count = 1;
        ExpectInput = expectInput;
        if (menuItems is null) return;
        foreach (var menuItem in menuItems.Where(s=> s is not null))
        {
            _listData.Add(count, menuItem!);
            count++;
        }
    }

    public string Header { get; init; } = string.Empty;
    private readonly Dictionary<int, string> _listData = new();

    public string GetStringWithoutHeader()
    {
        StringBuilder outString = new StringBuilder();
        foreach (var data in _listData)
        {
            outString.AppendLine($"{data.Key}{Seperator} {data.Value}");
        }
        if (_hasPrevPage)
            outString.AppendLine($"{BackCharacter ?? StringConstants.PREV_PAGE}{Seperator} Back");
        if (_hasNextPage)
            outString.AppendLine($"{NextCharacter ?? StringConstants.NEXT_PAGE}{Seperator} Next Page");
        return outString.ToString();
    }


    #region Overrides of Object

    /// <inheritdoc />
    public override string ToString()
    {
        StringBuilder outString = new();
        if (!string.IsNullOrEmpty(Header))
        {
            outString.AppendLine(Header);
        }

        outString.Append( GetStringWithoutHeader());
        return outString.ToString().Trim("\n\t ".ToCharArray());

    }

    #endregion

}