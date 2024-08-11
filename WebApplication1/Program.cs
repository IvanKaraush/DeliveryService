using Domain.Models.ApplicationModels;
using Domain.Stores;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Persistence;
using Persistence.Repository;
namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("DBOptions.json");
            builder.Services.Configure<MongoDBOptions>(builder.Configuration.GetSection(MongoDBOptions.MongoOptions));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<SQLContext>(options=> 
            {
                options.UseSqlServer(builder.Configuration.GetSection("SQLConnectionStrings").GetValue<string>("DefaultConnection")/*, sqlOptions=> sqlOptions.UseDateOnlyTimeOnly()*/);
            });
            builder.Services.AddSingleton<IMongoContext, MongoContext>();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetSection("RedisConnectionOptions").GetValue<string>("Configuration");
                options.InstanceName = builder.Configuration.GetSection("RedisConnectionOptions").GetValue<string>("InstanceName");
            });
            builder.Services.AddSingleton<ISoldProductStore, SoldProductRepository>();
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions()
            {
                RequestPath = "/files"
            });

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
