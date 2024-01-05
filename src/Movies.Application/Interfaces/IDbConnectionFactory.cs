using System.Data;

namespace Movies.Application.Interfaces;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnection();
}