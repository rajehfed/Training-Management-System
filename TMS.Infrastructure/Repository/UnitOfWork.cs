// File: TMS.Infrastructure/Repository/UnitOfWork.cs
using System.Data;
using TMS.Application.Shared.Interfaces;
using TMS.Infrastructure.Dapper;

namespace TMS.Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly IDbConnectionFactory _connectionFactory;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;

    public UnitOfWork(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IDbConnection Connection => _connection ??= _connectionFactory.CreateConnection();
    public IDbTransaction? Transaction => _transaction;

    public void BeginTransaction()
    {
        _connection ??= _connectionFactory.CreateConnection();

        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }

        _transaction ??= _connection.BeginTransaction();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _transaction?.Commit();
            return Task.FromResult(1);
        }
        catch
        {
            _transaction?.Rollback();
            throw;
        }
        finally
        {
            Dispose();
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _transaction = null;

        if (_connection != null)
        {
            if (_connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }
            _connection.Dispose();
            _connection = null;
        }
    }
}