using System.Data;

namespace TMS.Infrastructure.Dapper;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
