﻿using Dapper;
using Movies.Application.Interfaces;

namespace Movies.Application.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public RatingRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> RateMovie(Guid movieId, int rating, Guid userId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnection(token);
        var result = await connection.ExecuteAsync(new CommandDefinition("""
            insert into ratings(userid, movieid, rating)
            values (@userId, @movieId, @rating)
            on conflict (userid, movieid) do update
                set rating = @rating
            """, new { userId, movieId, rating }, cancellationToken: token));
        
        return result > 0;
    }

    public async Task<float?> GetRating(Guid movieId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnection(token);
        return await connection.QuerySingleOrDefaultAsync<float?>(new CommandDefinition("""
            select round(avg(r.rating), 1) from ratings r
            where movieid = @movieId
            """, new { movieId }, cancellationToken: token));
    }

    public async Task<(float? Rating, int? UserRating)> GetRating(Guid movieId, Guid userId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnection(token);
        return await connection.QuerySingleOrDefaultAsync<(float?, int?)>(new CommandDefinition("""
            select round(avg(r.rating), 1),
                (select ratings
                from ratings
                where movieid = @movieId
                    and userid = @userId
                limit 1)
            from ratings
            where movieid = @movieId
            """, new { movieId, userId }, cancellationToken: token));
    }
}