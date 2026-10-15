using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_Management_API.Errors
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, $"An unhandled exception occurred: ${exception.Message}");

            var statusCode = StatusCodes.Status500InternalServerError;
            var message = "An internal server error occurred.";

            switch(exception)
            {
                case KeyNotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    message = exception.Message;
                    break;

                case InvalidOperationException:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = exception.Message;
                    break;
            }

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            var response = new ProblemDetails
            {
                Status = statusCode,
                Title = statusCode == 404 ? "Not Found" : "Bad Request",
                Detail = message,
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}
