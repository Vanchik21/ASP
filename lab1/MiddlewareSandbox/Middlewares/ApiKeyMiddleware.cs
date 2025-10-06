using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MiddlewareSandbox.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _apiKey;
        private readonly ILogger<ApiKeyMiddleware> _logger;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<ApiKeyMiddleware> logger)
        {
            _next = next;
            _apiKey = configuration["ApiSettings:ApiKey"];
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Query.ContainsKey("custom"))
            {
                await _next(context);
                return;
            }

            if (context.Request.Path.Equals("/request-count"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("X-API-KEY", out var extractedKey) || string.IsNullOrWhiteSpace(extractedKey))
            {
                _logger.LogWarning("API Key missing");
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("API Key missing");
                await context.Response.CompleteAsync();
                return;
            }

            if (!string.Equals(_apiKey, extractedKey, System.StringComparison.Ordinal))
            {
                _logger.LogWarning("Invalid API Key: {Key}", extractedKey);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Invalid API Key");
                await context.Response.CompleteAsync();
                return;
            }

            _logger.LogInformation("API Key is valid: {Key}", extractedKey);
            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsync("API Key is valid");
            await context.Response.CompleteAsync();
            return;
        }
    }

    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }  
}