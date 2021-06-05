using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace StudentAPI.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider _provider;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IConfiguration config, ILoggerFactory loggerFactory)
        {
            _provider = provider;
            _config = config;
            _logger = loggerFactory.CreateLogger("Swagger");
        }

        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            try
            {
                foreach (var description in _provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, _config));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while configuring ConfigureSwaggerOptions", ex);
                throw ex;
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, IConfiguration configuration)
        {
            var info = new OpenApiInfo()
            {
                Title = configuration["SwaggerSettings:Title"],
                Version = description.ApiVersion.ToString(),
                Description = configuration["SwaggerSettings:Description"],
                Contact = new OpenApiContact() { Name = configuration["SwaggerSettings:ContactName"], Email = configuration["SwaggerSettings:ContactEmail"] }
                //License = new OpenApiLicense() { Name = configuration["SwaggerSettings:LicenseName"], Url = new Uri(configuration["SwaggerSettings:LicenseUrl"]) }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
