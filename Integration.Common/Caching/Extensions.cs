using Integration.Common.Caching.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Common.Caching
{
    public static class Extensions
    {
        public static IServiceCollection AddRedisCaching(this IServiceCollection services, IConfiguration config)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
                {
                    EndPoints =
                    {
                        { config.GetValue<string>("CacheSettings:ConnectionString"), config.GetValue<int>("CacheSettings:ReadPort") },
                        { config.GetValue<string>("CacheSettings:ConnectionString"), config.GetValue<int>("CacheSettings:WritePort") }
                    },
                    ClientName = config.GetValue<string>("CacheSettings:UserName"),
                    Password = config.GetValue<string>("CacheSettings:Password")
                };
            });

            services.AddSingleton<IResponseCacheService, ResponseCacheService>();

            return services;
        }
    }
}
