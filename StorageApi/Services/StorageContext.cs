using Microsoft.EntityFrameworkCore;
using StorageApi.Models.DBO.Storage;

namespace StorageApi.Services
{
    public class StorageContext : DbContext
    {
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<OfferStock> OfferStocks { get; set; }

        public StorageContext(DbContextOptions<StorageContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Offer>().HasAlternateKey(o => new { o.Product, o.Color, o.Size });
            modelBuilder.Entity<Product>().HasAlternateKey(p => p.Name);
            modelBuilder.Entity<Store>().HasAlternateKey(s => s.Name);
            modelBuilder.Entity<Brand>().HasAlternateKey(b => b.Name);
            modelBuilder.Entity<OfferStock>().HasAlternateKey(os => new { os.Offer, os.Store });
        }
    }
}
