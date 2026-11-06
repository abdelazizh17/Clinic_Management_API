using Hangfire.Dashboard;

namespace Clinic_Management_API.Infrastructure.Security
{
    /// <summary>
    /// Protects the Hangfire dashboard in non-development environments.
    /// Only an authenticated user with the Admin role can open /hangfire.
    /// </summary>
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            return httpContext.User.Identity?.IsAuthenticated == true &&
                   httpContext.User.IsInRole("Admin");
        }
    }
}
