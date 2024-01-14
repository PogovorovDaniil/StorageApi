using Microsoft.EntityFrameworkCore;
using StorageApi.Models.DBO.Storage;

namespace StorageApi.Services
{
    public class StorageContext : DbContext
    {
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferStock> OfferStocks { get; set; }

        public StorageContext(DbContextOptions<StorageContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
