using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace MongoDb.Extensions.Options
{
    public static class MongodbConfigExtension
    {
        public static IServiceCollection AddMongoDbConfig(
            this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MongoDbDataSettings>(
                config.GetSection("MongoDbDataSettings"));
            return services;
        }
    }
}
