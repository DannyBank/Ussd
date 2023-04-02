using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Dbank.Digisoft.PrediBet.Ussd.Data.Abstractions
{
    public interface IDbHelper
    {
        NpgsqlCommand CreateCommand(string commandName, string connectionStr = "");
        IDbConnection CreateConnection(string connectionStr = "");
        OracleCommand CreateOracleCommand(string commandString, string connectionProp, CommandType type = CommandType.StoredProcedure);
        IDbConnection CreateOracleConnection(string connectionStr = "");
        NpgsqlCommand LoadStoredProc(string storedProcName);
    }
}