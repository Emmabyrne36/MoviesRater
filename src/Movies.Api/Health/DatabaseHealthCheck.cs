using Microsoft.Extensions.Diagnostics.HealthChecks;
using Movies.Application.Interfaces;

namespace Movies.Api.Health;

public class DatabaseHealthCheck : IHealthCheck
{
    public const string HealthCheck = "Database";

    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<DatabaseHealthCheck> _logger;

    public DatabaseHealthCheck(IDbConnectionFactory connectionFactory, ILogger<DatabaseHealthCheck> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            _ = await _connectionFactory.CreateConnection(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            var errorMessage = "Database is unhealthy";
            _logger.LogError(errorMessage, ex);
            return HealthCheckResult.Unhealthy(errorMessage, ex);
        }
    }
}
