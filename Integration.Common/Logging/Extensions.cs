using Integration.Common.Logging.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Integration.Common.Logging
{
    public static class Extensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
