using System.Data;
using Movies.Application.Interfaces;
using Npgsql;

namespace Movies.Application.Database;

public class NpgSqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public NpgSqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}