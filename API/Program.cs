using API.DIConfiguration;
using API.Middleware;
using Domain.Models.ApplicationModels;
using Domain.Stores;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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
            CreateDirectories();
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMemoryCache();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Enter JWT token.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                        },
                        new List<string>()
                    }
                });
            });
            builder.ConfigureOptions();
            builder.ConfigureDatabases();
            builder.ConfigureRepositories();
            builder.ConfigureServices();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireUserRole", policy => policy.RequireRole("UserRole", "AdminRole"));
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("AdminRole"));
                options.AddPolicy("RequireHostRole", policy => policy.RequireRole("HostRole"));
                options.AddPolicy("RequireRestaurantRole", policy => policy.RequireRole("RestaurantRole", "HostRole"));
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.Client,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,

                    };
                    options.SaveToken = true;
                }
            );
            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ResizeMiddleware>();
            app.UseStaticFiles(new StaticFileOptions()
            {
                RequestPath = "/files"
            });
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseHttpsRedirection();



            app.MapControllers();
            app.Run();
        }
        private static void CreateDirectories()
        {
            string[] paths = "wwwroot\\images\\goods".Split("\\");
            for (int i = 1; i < paths.Length; i++)
            {
                paths[i] = $"{paths[i - 1]}\\{paths[i]}";
            }
            foreach (string path in paths)
            {
                DirectoryInfo drinfo = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\" + path);
                if (!drinfo.Exists)
                {
                    drinfo.Create();
                }
            }
        }
    }
}
