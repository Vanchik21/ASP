using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MiddlewareSandbox.Middlewares
{
    public class CustomQueryMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomQueryMiddleware> _logger;

        public CustomQueryMiddleware(RequestDelegate next, ILogger<CustomQueryMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Get &&
                context.Request.Query.TryGetValue("custom", out var customValue) &&
                !string.IsNullOrWhiteSpace(customValue))
            {
                _logger.LogInformation("Custom middleware triggered for value: {Value}", customValue);
                await context.Response.WriteAsync("You’ve hit a custom middleware!");
                await context.Response.CompleteAsync();
                return;
            }

            await _next(context);
        }
    }

    public static class CustomQueryMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomQuery(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomQueryMiddleware>();
        }
    }
}
