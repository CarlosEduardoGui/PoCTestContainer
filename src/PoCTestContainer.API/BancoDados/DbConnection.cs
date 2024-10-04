using System.Data;
using Dapper;
using PoCTestContainer.API.Interfaces;

namespace PoCTestContainer.API.BancoDados;

public class DbConnection : ISqlInterface
{
    private readonly IDbConnection _connection;

    public DbConnection(IDbConnection connection)
    {
        _connection = connection;
        if (connection.State != ConnectionState.Open)
            _connection.Open();
    }

    public async Task<int> Execute(string sql, object? parametros = null, CommandType? commandType = null)
    {
        return await _connection.ExecuteScalarAsync<int>(sql, parametros);
    }

    public async Task<IEnumerable<T>> QueryArrayResult<T>(string sql, object? parametros = null, int? commandTimeout = null)
    {
        return await _connection.QueryAsync<T>(sql, parametros);
    }

    public async Task<T?> QueryResult<T>(string sql, object? parametros = null, int? commandTimeout = null)
    {
        return await _connection.QueryFirstOrDefaultAsync<T>(sql, parametros);
    }
}
