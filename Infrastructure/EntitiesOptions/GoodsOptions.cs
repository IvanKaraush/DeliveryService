using Domain.Models.Entities.SQLEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntitiesOptions
{
    internal static class GoodsOptions
    {
        internal static void ApplyGoodsOptions(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Goods");
            modelBuilder.Entity<Product>().HasKey(p => p.Article);
            modelBuilder.Entity<Product>().HasIndex(p => p.Article).IsUnique();
            modelBuilder.Entity<Product>().Property(p => p.Price).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Title).IsRequired().HasMaxLength(32);
            modelBuilder.Entity<Product>().Property(p => p.AlreadyCooked).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Visible).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.ImageName).HasMaxLength(32);
        }
    }
}
