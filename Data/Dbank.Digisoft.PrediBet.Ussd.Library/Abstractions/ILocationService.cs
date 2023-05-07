using Acs.Mtn.Liberia.SDK.Ussd.Model.Location;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Abstractions;

public interface ILocationService
{
    Task<bool> IsAllowedLocation(LocationParameters locParams);
    Task<LocationModel?> GetLocation(string cgi);
}