using Domain.Models.ApplicationModels;

namespace API.DIConfiguration
{
    public static class ConfigureOptions
    {
        public static void ConfigureOpts(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("DBOptions.json");
            builder.Configuration.AddJsonFile("ConstsOptions.json");
            builder.Services.Configure<MongoDBOptions>(builder.Configuration.GetSection(MongoDBOptions.MongoOptions));
            builder.Services.Configure<ReposOptions>(builder.Configuration.GetSection(ReposOptions.RepositoryOptions));
            builder.Services.Configure<HostAuthOptions>(builder.Configuration.GetSection(HostAuthOptions.HostAuth));
            builder.Services.Configure<TelegramAPIOptions>(builder.Configuration.GetSection(TelegramAPIOptions.TelegramOptions));
            builder.Services.Configure<ServsOptions>(builder.Configuration.GetSection(ServsOptions.ServicesOptions));
        }
    }
}
