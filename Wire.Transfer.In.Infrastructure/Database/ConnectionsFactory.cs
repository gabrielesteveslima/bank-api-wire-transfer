using System;
using System.Data;
using System.Data.SqlClient;
using MongoDB.Driver;
using Wire.Transfer.In.Application.Configuration.Data;

namespace Wire.Transfer.In.Infrastructure.Database
{
    public class ConnectionsFactory : IConnectionFactory, IDisposable
    {
        private readonly string _cqrsConnectionString;
        private readonly string _sqlConnectionString;

        private IDbConnection _connection;

        public ConnectionsFactory(string sqlConnectionString, string cqrsConnectionString)
        {
            _sqlConnectionString = sqlConnectionString;
            _cqrsConnectionString = cqrsConnectionString;
        }

        public IDbConnection GetOpenSqlConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new SqlConnection(_sqlConnectionString);
                _connection.Open();
            }

            return _connection;
        }

        public IMongoDatabase GetCqrsConnection()
        {
            var databaseName = MongoUrl.Create(_cqrsConnectionString);

            var client = new MongoClient(databaseName);
            return client.GetDatabase(databaseName.DatabaseName);
        }

        public void Dispose()
        {
            if (_connection != null && _connection.State == ConnectionState.Open) _connection.Dispose();
        }
    }
}