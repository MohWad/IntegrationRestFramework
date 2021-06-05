using Integration.Common.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace APITemplate.Swagger
{
    public static class Extensions
    {
        public static IServiceCollection AddAndConfigureSwagger(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();

                try
                {
                    // integrate xml comments
                    var xmlCommentsFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                    options.IncludeXmlComments(xmlCommentsFullPath);
                }
                catch (Exception ex)
                {
                }

                //// will add secuirty info to the doc and provide the authorize button and form in the UI
                var authType = config["AuthenticationSettings:AuthenticationType"]?.ToLower();
                var scheme = config["AuthenticationSettings:AuthenticationScheme"]?.ToLower();
                if (!string.IsNullOrEmpty(authType) || authType != "none")
                {
                    options.AddSecurityDefinition(scheme, GetOpenApiSecurityScheme(config));
                }

                // will send provided username and password in authorization header
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = scheme }
                        }, new List<string>() 
                    }
                });

                options.AddFluentValidationRules();
            });

            return services;
        }

        public static void UseSwaggerAndSwaggerUI(this IApplicationBuilder app, ILoggerFactory loggerfactory, IApiVersionDescriptionProvider provider)
        {
            var logger = loggerfactory.CreateLogger("Swagger");
            try
            {
                app.UseSwagger();

                app.UseSwaggerUI(
                    options =>
                    {
                        // build a swagger endpoint for each discovered API version
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint($"./swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                        }
                        options.RoutePrefix = "";
                    });
            }
            catch (Exception ex)
            {
                logger.LogError("UseSwaggerAndSwaggerUI Error");
                throw ex;
            }

        }

        public static void AddSwaggerStatusCodeFilters(this MvcOptions options)
        {
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResponse), StatusCodes.Status400BadRequest));
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(object), StatusCodes.Status406NotAcceptable));
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(object), StatusCodes.Status500InternalServerError));
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(object), StatusCodes.Status401Unauthorized));
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(object), StatusCodes.Status403Forbidden));
        }

        public static OpenApiSecurityScheme GetOpenApiSecurityScheme(IConfiguration config)
        {
            return new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri(config["AuthenticationSettings:TokenEndpoint"])
                    }
                }
            };
        }
    }
}
