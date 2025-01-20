namespace BookStore.Middleware
{
    public class ErrorHandling
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandling> _logger;

        public ErrorHandling(RequestDelegate next, ILogger<ErrorHandling> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                _logger.LogError(ex.Message);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                context.Response.Redirect("/Books/Error");
            }
        }
    }
}
