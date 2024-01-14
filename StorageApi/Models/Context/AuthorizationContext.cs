using Microsoft.EntityFrameworkCore;
using StorageApi.Models.DBO.Authorization;

namespace StorageApi.Models.Context
{
    public class AuthorizationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AuthorizationContext(DbContextOptions<AuthorizationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasAlternateKey(u => u.Login);
        }
    }
}
