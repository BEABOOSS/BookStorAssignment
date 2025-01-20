using System.Diagnostics;

namespace BookStore.Middleware
{
    public class RequestDurationFilter
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestDurationFilter> _logger;
        private readonly string _instanceId;

        // Constructor injection of dependencies
        public RequestDurationFilter(RequestDelegate next, ILogger<RequestDurationFilter> logger)
        {
            _next = next;
            _logger = logger;
            // Generate a unique ID for this middleware instance
            _instanceId = Guid.NewGuid().ToString("N")[..8];
        }

        // The InvokeAsync method is required for all middleware
        public async Task InvokeAsync(HttpContext context)
        {
            // Start timer before processing request
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Create a scope for structured logging
                using var scope = _logger.BeginScope(new Dictionary<string, object>
                {
                    ["CorrelationId"] = context.TraceIdentifier,
                    ["MiddlewareInstance"] = _instanceId
                });

                // Log the start of request
                _logger.LogInformation(
                    "Request started: {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                // Call next middleware in the pipeline
                await _next(context);

                // Stop timing and log the duration
                stopwatch.Stop();

                _logger.LogInformation(
                    "Request completed: {Method} {Path} - Duration: {Duration}ms, Status: {StatusCode}",
                    context.Request.Method,
                    context.Request.Path,
                    stopwatch.ElapsedMilliseconds,
                    context.Response.StatusCode);
            }
            catch (Exception ex)
            {
                // Ensure log duration even if error occurs
                stopwatch.Stop();

                _logger.LogError(
                    ex,
                    "Request failed: {Method} {Path} - Duration: {Duration}ms",
                    context.Request.Method,
                    context.Request.Path,
                    stopwatch.ElapsedMilliseconds);

                // Re-throw the exception to be handled by error handling middleware
                throw;
            }
        }
    }

    // Extension methods for cleaner middleware registration
    public static class RequestDurationMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestDuration(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestDurationFilter>();
        }
    }
}
