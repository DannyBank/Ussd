using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using Dbank.Digisoft.PrediBet.Ussd.Data.Abstractions;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Dbank.Digisoft.PrediBet.Ussd.Data.Models
{
    public class DbHelper : IDisposable, IDbHelper
    {
        public static DbHelper Instance = new DbHelper();

        private readonly IConfiguration _configuration;
        private IDbConnection _connection;

        public DbHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbHelper()
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_connection is not null && _connection.State is ConnectionState.Open)
                _connection.Close();

            _connection?.Dispose();
        }

        ~DbHelper()
        {
            Dispose(false);
        }

        public NpgsqlCommand LoadStoredProc(string storedProcName)
        {
            return CreateCommand(storedProcName);
        }

        public NpgsqlCommand CreateCommand(string commandName, string connectionStr = "")
        {
            if (_configuration == null) return CreateCommandOld(commandName, connectionStr);
            connectionStr =
                _configuration.GetConnectionString(
                    string.IsNullOrWhiteSpace(connectionStr) ? "Database" : connectionStr);
            var connection = new NpgsqlConnection(connectionStr);
            var command = new NpgsqlCommand(commandName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            return command;
        }

        public OracleCommand CreateOracleCommand(string commandString, string connectionProp,
            CommandType type = CommandType.StoredProcedure)
        {
            if (_configuration == null) return CreateOracleCommandOld(commandString, connectionProp, type);
            var connectionStr =
                _configuration.GetConnectionString(string.IsNullOrWhiteSpace(connectionProp)
                    ? "Database"
                    : connectionProp);
            var connection = new OracleConnection(connectionStr);
            var command = new OracleCommand(commandString, connection)
            {
                CommandType = type
            };
            return command;
        }

        private NpgsqlCommand CreateCommandOld(string commandName, string connectionStr = "")
        {
            connectionStr = ConfigurationManager
                .ConnectionStrings[string.IsNullOrWhiteSpace(connectionStr) ? "Database" : connectionStr]
                .ConnectionString;
            var connection = new NpgsqlConnection(connectionStr);
            var command = new NpgsqlCommand(commandName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            return command;
        }

        private OracleCommand CreateOracleCommandOld(string commandName, string connectionStr = "",
            CommandType commandType = CommandType.StoredProcedure)
        {
            connectionStr = ConfigurationManager
                .ConnectionStrings[string.IsNullOrWhiteSpace(connectionStr) ? "Database" : connectionStr]
                .ConnectionString;
            var connection = new OracleConnection(connectionStr);
            var command = new OracleCommand(commandName, connection)
            {
                CommandType = commandType
            };
            return command;
        }

        #region Connection Function

        [DebuggerHidden]
        public IDbConnection CreateConnection(string connectionStr = "")
        {
            if (_configuration == null) return CreateConnectionOld(connectionStr);

            connectionStr =
                _configuration.GetConnectionString(
                    string.IsNullOrWhiteSpace(connectionStr) ? "Database" : connectionStr);
            _connection = new NpgsqlConnection(connectionStr);
            return _connection;
        }
        public IDbConnection CreateOracleConnection(string connectionStr = "")
        {
            if (_configuration == null) return CreateConnectionOracleOld(connectionStr);

            connectionStr =
                _configuration.GetConnectionString(
                    string.IsNullOrWhiteSpace(connectionStr) ? "Database" : connectionStr);
            _connection = new OracleConnection(connectionStr);
            return _connection;
        }

        private IDbConnection CreateConnectionOld(string connectionStr = "")
        {
            connectionStr = ConfigurationManager
                .ConnectionStrings[string.IsNullOrWhiteSpace(connectionStr) ? "Database" : connectionStr]
                .ConnectionString;

            _connection = new NpgsqlConnection(connectionStr);
            return _connection;
        } 
        private IDbConnection CreateConnectionOracleOld(string connectionStr = "")
        {
            connectionStr = ConfigurationManager
                .ConnectionStrings[string.IsNullOrWhiteSpace(connectionStr) ? "Database" : connectionStr]
                .ConnectionString;

            _connection = new OracleConnection(connectionStr);
            return _connection;
        }

        #endregion
    }
}