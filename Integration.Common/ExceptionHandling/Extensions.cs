using Integration.Common.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Net;

namespace Integration.Common.ExceptionHandling
{
    public static class Extensions
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger("Exception");

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var clientId = context?.User.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value ?? "Anonymous";

                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                        logger.LogError(contextFeature.Error, contextFeature.Error.Message);
                    else
                        logger.LogError(null, "An Unknown Exception has occuured");

                    //var errorResponse = new ErrorResponse(new Error { Code = "500", Message = "Generic failure, Please contact integration team" });
                    //var errorResponseJson = JsonConvert.SerializeObject(errorResponse);
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync(string.Empty);
                });
            });
        }
    }
}
