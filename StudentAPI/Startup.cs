using IdentityServer4.AccessTokenValidation;
using Integration.Common.Caching;
using Integration.Common.ContentNegotiation;
using Integration.Common.ExceptionHandling;
using Integration.Common.Logging;
using Integration.Common.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StudentAPI.Extensions;
using StudentAPI.Swagger;

namespace StudentAPI
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
                .AddAndConfigureSwagger(Configuration)
            ;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            app.UseGlobalExceptionHandler(loggerFactory);
            
            if (Configuration.GetValue<bool>("EnableRequestResponseLogging"))
            {
                app.UseRequestResponseLogging();
            }

            app.UseHttpsRedirection();

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
