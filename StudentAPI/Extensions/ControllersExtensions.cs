using FluentValidation.AspNetCore;
using Integration.Common.Validation.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using StudentAPI.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Extensions
{
    public static class ControllersExtensions
    {
        public static IServiceCollection AddAndConfigControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                //options.Filters.Add(typeof(ContentNegotiationFilter));
                options.Filters.Add(typeof(ValidationFilter));

                // add swagger status codes response
                options.AddSwaggerStatusCodeFilters();

                // returns "406 Not Acceptable" status code when the media type in the "Accept" header is not supported (e.g. "application/xml")
                options.ReturnHttpNotAcceptable = true;

                // remove support of "text/json" media type
                var jsonOutputFormatter = options.OutputFormatters.OfType<SystemTextJsonOutputFormatter>().FirstOrDefault();
                if (jsonOutputFormatter != null)
                {
                    if (jsonOutputFormatter.SupportedMediaTypes.Contains("text/json"))
                    {
                        jsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
                    }
                }
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                // supress the automatic generic validation done by [ApiController]
                options.SuppressModelStateInvalidFilter = true;
            })
            .AddFluentValidation(op =>
            {
                op.RegisterValidatorsFromAssemblyContaining<Startup>();
                op.ImplicitlyValidateChildProperties = true;
            });

            return services;
        }
    }
}
