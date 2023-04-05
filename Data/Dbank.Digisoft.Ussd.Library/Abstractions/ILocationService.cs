using Dbank.Digisoft.Ussd.SDK.Model.Location;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Ussd.SDK.Abstractions;

public interface ILocationService
{
    Task<bool> IsAllowedLocation(LocationParameters locParams);
    Task<LocationModel?> GetLocation(string cgi);
}