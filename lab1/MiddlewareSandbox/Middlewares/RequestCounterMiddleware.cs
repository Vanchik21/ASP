using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MiddlewareSandbox.Middlewares
{
    public class RequestCounterMiddleware
    {
        private readonly RequestDelegate _next;
        private static int _requestCount = 0;

        public RequestCounterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Interlocked.Increment(ref _requestCount);

            if (context.Request.Path.Equals("/request-count"))
            {
                await context.Response.WriteAsync($"The amount of processed requests is {_requestCount}.");
                await context.Response.CompleteAsync();
                return;
            }

            await _next(context);
        }
    }

    public static class RequestCounterMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCounter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCounterMiddleware>();
        }
    }
}
