using Integration.Common.Caching.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Common.Caching.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CachingFilter : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLive;

        public CachingFilter(int timeToLive)
        {
            _timeToLive = timeToLive;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var loggerfactory = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerfactory.CreateLogger("Caching");

            var clientId = context.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value ?? "Anonymous";

            try
            {
                // check if request is cached, if not continue to the controller "await next()"
                var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

                // use the endpoint uri as key. query strings are ordered first and then added to uri to avoid cacheing the same request with
                // the same request with different query string ordering
                var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
                var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedResponse))
                {
                    logger.LogInformation("Retrieved Response from Cache successfully - [[{ClientName}]]", clientId);

                    var contentResult = new ContentResult
                    {
                        Content = cachedResponse,
                        ContentType = "application/json",
                        StatusCode = 200
                    };
                    context.Result = contentResult;
                    return;
                }

                // CONTINUE TO THE CONTROLLER
                var executedContext = await next();

                // AFTER
                // if the response from the action is Ok (200), cache the response
                if (executedContext.Result is OkObjectResult okObjectResult)
                {
                    await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLive));
                    logger.LogInformation("Stored Response in Cache successfully - [[{ClientName}]]", clientId);

                }
            }
            catch (Exception ex)
            {
                logger.LogError("Caching Error - {@Error} - [[{ClientName}]]", ex, clientId);

                await next();
            }
        }

        private static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var keyVal in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{keyVal.Key}-{keyVal.Value}");
            }

            return keyBuilder.ToString();
        }
    }
}
