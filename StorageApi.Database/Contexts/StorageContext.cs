using Microsoft.EntityFrameworkCore;
using StorageApi.Database.Models.Storage;

namespace StorageApi.Database.Contexts
{
    public class StorageContext : DbContext
    {
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<OfferStock> OfferStocks { get; set; }

        public StorageContext(DbContextOptions<StorageContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>().HasAlternateKey(s => s.Name);
            modelBuilder.Entity<Brand>().HasAlternateKey(b => b.Name);

            modelBuilder.Entity<Product>().HasAlternateKey(p => p.Name);
            modelBuilder.Entity<Product>().HasMany(p => p.Offers).WithOne(o => o.Product);

            modelBuilder.Entity<Offer>().HasAlternateKey(new string[] { "ProductId", "Color", "Size" });
            modelBuilder.Entity<OfferStock>().HasKey(new string[] { "OfferId", "StoreId" });
        }
    }
}
