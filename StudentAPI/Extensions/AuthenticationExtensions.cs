using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Extensions
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
                options.AddPolicy("GetStudents", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireScope("getstudents");
                });
                options.AddPolicy("CreateStudent", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireScope("createstudent");
                });
                options.AddPolicy("CreateStudentV2", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireScope("createstudentv2");
                });
            });
            return services;
        }
    }
}
