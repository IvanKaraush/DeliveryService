using API.DIConfiguration;
using Domain.Models.ApplicationModels;
using Domain.Stores;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Persistence;
using Persistence.Repository;
using Telegram.Bot;
namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("DBOptions.json");
            builder.Configuration.AddJsonFile("ConstsOptions.json");
            builder.Services.Configure<MongoDBOptions>(builder.Configuration.GetSection(MongoDBOptions.MongoOptions));
            builder.Services.Configure<ReposOptions>(builder.Configuration.GetSection(ReposOptions.RepositoryOptions));
            builder.Services.Configure<HostAuthOptions>(builder.Configuration.GetSection(HostAuthOptions.HostAuth));
            builder.Services.Configure<TelegramAPIOptions>(builder.Configuration.GetSection(TelegramAPIOptions.TelegramOptions));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<ITelegramBotApi, TelegramBotAPI>();
            builder.Services.AddHostedService<BotService>();
            builder.ConfigureDBs();
            builder.ConfigureReposes();            
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
