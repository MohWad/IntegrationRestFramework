using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APITemplate.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = config["AuthenticationSettings:Authority"];
                    options.ApiName = config["AuthenticationSettings:ApiName"];
                    options.ApiSecret = config["AuthenticationSettings:ApiSecret"];
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("PolicyName", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireScope("ScopeName");
                    policyBuilder.RequireScope("ScopeName");
                });
                options.AddPolicy("PolicyName", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireScope("ScopeName");
                });
            });

            return services;
        }
    }
}
