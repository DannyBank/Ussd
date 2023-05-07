namespace Dbank.Digisoft.Ussd.SDK.Model.Location;

public class LocationParameters
{
    private int? _MCC;
    private int? _MNC;
    private int? _LAC;
    private int? _CellId;
    public LocationParameters(int? MCC, int? MNC, int? LAC, int? CellId)
    {
        _MCC = MCC;
        _MNC = MNC;
        _LAC = LAC;
        _CellId = CellId;
    }
    public string CGI 
    {
        get {
            if (_MCC is null || _MNC is null || _LAC is null || _CellId is null)
            {
                return string.Empty;
            }

            return $"{_MCC}-0{_MNC}-{_LAC}-{_CellId}";
        }
    }
}