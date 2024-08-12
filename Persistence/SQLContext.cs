using Domain.Models.Entities.SQLEntities;
using Microsoft.EntityFrameworkCore;
using Persistence.EntitiesOptions;

namespace Persistence
{
    public class SQLContext : DbContext
    {
        public SQLContext(DbContextOptions<SQLContext> options) : base(options) { }
        public SQLContext() { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyUsersOptions();
            modelBuilder.ApplyCardsOptions();
            modelBuilder.ApplyRestaurantsOptions();
            modelBuilder.ApplyGoodsOptions();
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Product> Goods { get; set; }
    }
}
