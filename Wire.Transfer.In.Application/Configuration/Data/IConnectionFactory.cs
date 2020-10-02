using System.Data;
using MongoDB.Driver;

namespace Wire.Transfer.In.Application.Configuration.Data
{
    public interface IConnectionFactory
    {
        IDbConnection GetOpenSqlConnection();
        IMongoDatabase GetCqrsConnection();
    }
}