using Dapper;
using Movies.Application.Interfaces;

namespace Movies.Application.Database;

public class DbInitialiser
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DbInitialiser(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task Initialise()
    {
        using var connection = await _dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(
            """
             create table if not exists movies (
             id UUID primary key,
             slug TEXT not null,
             title TEXT not null,
             yearofrelease integer not null);
             """);

        await connection.ExecuteAsync("""
             create unique index concurrently if not exists movies_slug_idx
             on movies
             using btree(slug);
             """);
    }
}