using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.DIConfiguration
{
    public static class ConfigureDatabases
    {
        public static void ConfigureDBs(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<SQLContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetSection("SQLConnectionStrings").GetValue<string>("DefaultConnection")/*, sqlOptions=> sqlOptions.UseDateOnlyTimeOnly()*/);
            });
            builder.Services.AddSingleton<IMongoContext, MongoContext>();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetSection("RedisConnectionOptions").GetValue<string>("Configuration");
                options.InstanceName = builder.Configuration.GetSection("RedisConnectionOptions").GetValue<string>("InstanceName");
            });
        }
    }
}
