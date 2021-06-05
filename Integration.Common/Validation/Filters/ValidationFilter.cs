using Integration.Common.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integration.Common.Validation.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidationFilter : Attribute, IAsyncActionFilter
    {
        private readonly ILogger _logger;

        public ValidationFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("Validation");
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                var clientId = context?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value ?? "Anonymous";

                if (!context.ModelState.IsValid)
                {
                    ErrorResponse errorResponse = null;
                    if (context.HttpContext.Request.ContentLength == 0)
                    {
                        errorResponse = new ErrorResponse(new Error { Code = "0", Message = "Request body cannot be empty" });
                        _logger.LogInformation("Validation Failed- Request cannot be empty - [[{ClientName}]]", clientId);
                    }
                    else
                    {
                        var modelSatetErrors = context.ModelState.GetErrorList();
                        List<Error> errors = new List<Error>();
                        foreach (var modelSatetError in modelSatetErrors)
                        {
                            var errorCodeAndMessage = modelSatetError.Split("-");
                            if (errorCodeAndMessage.Count() == 1)
                                errors.Add(new Error { Message = errorCodeAndMessage[0] });
                            else
                                errors.Add(new Error { Code = errorCodeAndMessage[0].Trim(), Message = errorCodeAndMessage[1].Trim() });
                        }
                        errorResponse = new ErrorResponse(errors);
                        _logger.LogInformation("Validation Failed- {@Errors}", errors);
                    }

                    context.Result = new BadRequestObjectResult(errorResponse);
                }
                else
                {
                    _logger.LogInformation("Validation Successeded - [[{ClientName}]]", clientId);
                    await next();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Validation Filter Error");
                throw ex;
            }
        }
    }
}
