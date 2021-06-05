using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Common.ContentNegotiation.Middleware
{
    public class ContentNegotiationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ContentNegotiationMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger("ContentNegotiation");
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Accept"))
            {
                _logger.LogWarning("Missing Accept Header");
                context.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                await context.Response.WriteAsync("");
            }

            if (context.Request.Method == "POST" || context.Request.Method == "PUT" || context.Request.Method == "PATCH")
            {
                var contentTyep = context.Request.Headers["Content-Type"].ToString();
                    //if (!context.Request.Headers.ContainsKey("Content-Type"))
                if (contentTyep == null || contentTyep != "application/json")
                {
                    _logger.LogWarning("Missing Content-Type Header");
                    context.Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
                    await context.Response.WriteAsync("");
                }
            }

            await _next(context);
        }
    }
}
