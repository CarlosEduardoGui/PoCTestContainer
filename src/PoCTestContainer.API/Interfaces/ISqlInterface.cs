using System.Data;

namespace PoCTestContainer.API.Interfaces
{
    public interface ISqlInterface
    {
        Task<int> Execute(string sql, object? parametros = null, CommandType? commandType = null);
        Task<IEnumerable<T>> QueryArrayResult<T>(string sql, object? parametros = null, int? commandTimeout = null);
        Task<T?> QueryResult<T>(string sql, object? parametros = null, int? commandTimeout = null);
    }
}
