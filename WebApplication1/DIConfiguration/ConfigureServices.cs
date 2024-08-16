using Application.Interfaces;
using Application.Services;
using Infrastructure;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace API.DIConfiguration
{
    public static class ConfigureServices
    {
        public static void ConfigureServs(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAuditHostService, AuditHostService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICardsUserService, CardsUserService>();
            builder.Services.AddScoped<IGoodsAdminService, GoodsAdminService>();
            builder.Services.AddScoped<IGoodsUserService, GoodsUserService>();
            builder.Services.AddScoped<IOrderRestaurantService, OrderRestaurantService>();
            builder.Services.AddScoped<IOrderUserService, OrderUserService>();
            builder.Services.AddScoped<IReportAdminService, ReportAdminService>();
            builder.Services.AddScoped<IReportUserService, ReportUserService>();
            builder.Services.AddScoped<IRestaurantAdminService, RestaurantAdminService>();
            builder.Services.AddScoped<IRestaurantUserService, RestaurantUserService>();
            builder.Services.AddScoped<IUserHostService, UserHostService>();
            builder.Services.AddScoped<IUserUserService, UserUserService>();
            builder.Services.AddSingleton<ITelegramBotApi, TelegramBotAPI>();
            builder.Services.AddHostedService<BotService>();
            builder.Services.AddHostedService<DirectoryService>();
        }
    }
}
