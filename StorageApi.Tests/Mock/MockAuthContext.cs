using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StorageApi.Database.Contexts;

namespace StorageApi.Tests.Mock
{
    public class MockAuthContext
    {
        public AuthorizationContext AuthContext { get; }
        public MockAuthContext()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var contextOptions = new DbContextOptionsBuilder<AuthorizationContext>().UseSqlite(connection).Options;
            AuthContext = new AuthorizationContext(contextOptions);
            AuthContext.Database.EnsureCreated();
        }
    }
}
