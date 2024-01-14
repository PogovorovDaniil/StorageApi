using Microsoft.EntityFrameworkCore;
using StorageApi.Models.DBO.Authorization;

namespace StorageApi.Services
{
    public class AuthorizationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AuthorizationContext(DbContextOptions<AuthorizationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasAlternateKey(u => u.Login);
        }
    }
}
