using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace APITemplate.Extensions
{
    public static class LocalDependecyExtensions
    {
        public static IServiceCollection AddLocalDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            return services;
        }
    }
}
