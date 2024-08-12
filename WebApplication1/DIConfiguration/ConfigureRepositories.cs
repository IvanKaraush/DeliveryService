using Domain.Stores;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repository;

namespace API.DIConfiguration
{
    public static class ConfigureRepositories
    {
        public static void ConfigureReposes(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ISoldProductStore, SoldProductRepository>();
            builder.Services.AddSingleton<IAuditLogStore, AuditLogRepository>();
            builder.Services.AddSingleton<IOrderStore, OrderRepository>();
            builder.Services.AddSingleton<IReportStore, ReportRepository>();
            builder.Services.AddScoped<ICardStore, CardRepository>();
            builder.Services.AddScoped<IProductStore, ProductRepository>();
            builder.Services.AddScoped<IRestaurantStore, RestaurantRepository>();
            builder.Services.AddScoped<IUserStore, UserRepository>();
        }
    }
}
