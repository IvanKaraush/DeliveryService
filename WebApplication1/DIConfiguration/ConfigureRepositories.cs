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
            builder.Services.AddScoped<ISoldProductStore, SoldProductRepository>();
            builder.Services.AddScoped<IAuditLogStore, AuditLogRepository>();
            builder.Services.AddScoped<IOrderStore, OrderRepository>();
            builder.Services.AddScoped<IReportStore, ReportRepository>();
            builder.Services.AddScoped<ICardStore, CardRepository>();
            builder.Services.AddScoped<IProductStore, ProductRepository>();
            builder.Services.AddScoped<IRestaurantStore, RestaurantRepository>();
            builder.Services.AddScoped<IUserStore, UserRepository>();
        }
    }
}
