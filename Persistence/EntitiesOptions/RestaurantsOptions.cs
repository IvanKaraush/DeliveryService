using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.EntitiesOptions
{
    public static class RestaurantsOptions
    {
        public static void ApplyRestaurantsOptions(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>().ToTable("Restaurants");
            modelBuilder.Entity<Restaurant>().HasKey(r => r.Adress);
            modelBuilder.Entity<Restaurant>().HasIndex(r => r.Adress).IsUnique();
            modelBuilder.Entity<Restaurant>().HasIndex(r => r.Login).IsUnique();
            modelBuilder.Entity<Restaurant>().HasIndex(r => r.Password).IsUnique();
            modelBuilder.Entity<Restaurant>().Property(r => r.Adress).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Restaurant>().Property(r => r.Latitude).IsRequired();
            modelBuilder.Entity<Restaurant>().Property(r => r.Longitude).IsRequired();
            modelBuilder.Entity<Restaurant>().Property(r => r.Login).IsRequired().HasMaxLength(16);
            modelBuilder.Entity<Restaurant>().Property(r => r.Password).IsRequired().HasMaxLength(16);
        }
    }
}
