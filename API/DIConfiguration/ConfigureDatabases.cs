using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Infrastructure.Interfaces;

namespace API.DIConfiguration
{
    internal static partial class ConfigurationExtensions
    {
        internal static void ConfigureDatabases(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<SQLContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetSection("SQLConnectionStrings").GetValue<string>("DefaultConnection")/*, sqlOptions=> sqlOptions.UseDateOnlyTimeOnly()*/);
            });
            builder.Services.AddScoped<IMongoContext, MongoContext>();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetSection("RedisConnectionOptions").GetValue<string>("Configuration");
                options.InstanceName = builder.Configuration.GetSection("RedisConnectionOptions").GetValue<string>("InstanceName");
            });
        }
    }
}
