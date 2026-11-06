using Clinic_Management_API.Infrastructure.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Clinic_Management_API.Infrastructure.HealthChecks
{
    /// <summary>
    /// Simple health check that verifies the API can reach SQL Server.
    /// Exposed by Docker through the /health endpoint.
    /// </summary>
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly ClinicDBContext _context;

        public DatabaseHealthCheck(ClinicDBContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync(cancellationToken);

                return canConnect
                    ? HealthCheckResult.Healthy("Database is reachable.")
                    : HealthCheckResult.Unhealthy("Database is not reachable.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Database health check failed.", ex);
            }
        }
    }
}
