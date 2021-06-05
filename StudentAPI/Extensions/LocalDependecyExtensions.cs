using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SampleAPI.Services;

namespace StudentAPI.Extensions
{
    public static class LocalDependecyExtensions
    {
        public static IServiceCollection AddLocalDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IStudentService, StudentService>();

            services.AddAutoMapper(typeof(Startup));

            return services;
        }
    }
}
