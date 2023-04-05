using Dapper;
using Dbank.Digisoft.Ussd.Data.Models;
using Dbank.Digisoft.Ussd.SDK.Model.Location;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Ussd.SDK.Abstractions;

public class LccLocationService : ILocationService
{
    private readonly DbHelper _dbHelper;
    private readonly ILogger<LccLocationService> _logger;

    public LccLocationService(DbHelper dbHelper, ILogger<LccLocationService> logger)
    {
        _dbHelper = dbHelper;
        _logger = logger;
    }

    public async Task<bool> IsAllowedLocation(LocationParameters locParams)
    {
        var loc = await GetLocation(locParams.CGI);
        return (loc is not null);
    }

    public async Task<LocationModel?> GetLocation (string cgi)
    {
        try
        {
            using var connection = _dbHelper.CreateOracleConnection("AlexyDB");
            var query = $"SELECT CGI, SITE_NAME as SiteName, LOCATION as Location, " +
                        $"LATITUDE as Latitude, LONGITUDE as Longitude " +
                        $"FROM alexy.ALL_DIM_CGI WHERE CGI = :cgi ";

            var data = await connection.QueryAsync<LocationModel>(query, new { cgi });
            return data.SingleOrDefault();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error Executing {MethodName}", nameof(GetLocation));
            return null;
        }
            
    }


}