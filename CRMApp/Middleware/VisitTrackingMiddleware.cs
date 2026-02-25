using CRMApp.Services;

namespace CRMApp.Middleware
{
    public class VisitTrackingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<VisitTrackingMiddleware> _logger;

        public VisitTrackingMiddleware(RequestDelegate next, ILogger<VisitTrackingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IWebsiteVisitService visitService)
        {
            try
            {
                // Only track page visits (not API calls, static files, etc.)
                var path = context.Request.Path.Value?.ToLower() ?? "";
                
                if (!path.Contains("/api/") && 
                    !path.Contains("/lib/") && 
                    !path.Contains("/css/") && 
                    !path.Contains("/js/") &&
                    !path.Contains("/images/") &&
                    !path.Contains(".ico") &&
                    context.Request.Method == "GET")
                {
                    var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                    var userAgent = context.Request.Headers["User-Agent"].ToString();
                    var pageUrl = $"{context.Request.Path}{context.Request.QueryString}";

                    await visitService.LogVisitAsync(ipAddress, userAgent, pageUrl);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in VisitTrackingMiddleware");
            }

            await _next(context);
        }
    }

    public static class VisitTrackingMiddlewareExtensions
    {
        public static IApplicationBuilder UseVisitTracking(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<VisitTrackingMiddleware>();
        }
    }
}