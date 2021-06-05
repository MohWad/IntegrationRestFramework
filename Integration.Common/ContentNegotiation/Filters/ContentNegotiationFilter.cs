using Integration.Common.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Integration.Common.Validation.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ContentNegotiationFilter : Attribute, IAsyncActionFilter
    {
        private readonly ILogger _logger;

        public ContentNegotiationFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("ContentNegotiation");
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var accept = context.HttpContext.Request.Headers["Accept"].ToString();
                //if (!context.HttpContext.Request.Headers.ContainsKey("Accept"))
            if (accept == null || accept != "application/json")
            {
                _logger.LogWarning("Missing Accept Header");
                var response = new ObjectResult(null);
                response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                context.Result = new BadRequestObjectResult(response);
            }
            else
            {
                if (context.HttpContext.Request.Method == "POST" || context.HttpContext.Request.Method == "PUT" || context.HttpContext.Request.Method == "PATCH")
                {
                    var contentTyep = context.HttpContext.Request.Headers["Content-Type"].ToString();
                    //if (!context.HttpContext.Request.Headers.ContainsKey("Content-Type"))
                    if (contentTyep == null || contentTyep != "application/json")
                    {
                        _logger.LogWarning("Missing Content-Type Header");
                        var response = new ObjectResult(null);
                        response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
                        context.Result = new BadRequestObjectResult(response);
                    }
                    else
                    {
                        await next();
                    }
                }
                else
                {
                    await next();
                }
            }
        }
    }
}
