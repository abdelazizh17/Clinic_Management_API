using Microsoft.AspNetCore.Mvc;

namespace Clinic_Management_API.Errors
{
    public static class CustomStatusCodeMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomStatusCodePages(this IApplicationBuilder app)
        {
            return app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;

                if (response.StatusCode == StatusCodes.Status401Unauthorized ||
                    response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    response.ContentType = "application/json";

                    var title = response.StatusCode == 401 ? "Unauthorized" : "Forbidden";

                    var details = response.StatusCode == 401 
                    ? "You are not authenticated. Please login first." 
                    : "You do not have permission to access this resource.";

                    var problemDetails = new ProblemDetails
                    {
                        Status = response.StatusCode,
                        Title = title,
                        Detail = details
                    };

                    await response.WriteAsJsonAsync(problemDetails);
                }
            });
        }
    }
}
