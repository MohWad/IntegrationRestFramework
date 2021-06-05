using APITemplate.Extensions;
using APITemplate.Swagger;
using Integration.Common.Caching;
using Integration.Common.ExceptionHandling;
using Integration.Common.Logging;
using Integration.Common.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace APITemplate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddLocalDependencies()
                .AddAndConfigControllers()
                .AddAuthenticationAndAuthorization(Configuration)
                .AddRedisCaching(Configuration)
                .AddAndConfigureApiVersioning()
                .AddAndConfigureSwagger(Configuration);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            app.UseGlobalExceptionHandler(loggerFactory);

            if (Configuration.GetValue<bool>("EnableRequestResponseLogging"))
            {
                app.UseRequestResponseLogging();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerAndSwaggerUI(loggerFactory, apiVersionDescriptionProvider);
        }
    }
}
