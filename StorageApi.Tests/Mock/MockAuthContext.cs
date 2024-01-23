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
            var _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            var _contextOptions = new DbContextOptionsBuilder<AuthorizationContext>().UseSqlite(_connection).Options;
            AuthContext = new AuthorizationContext(_contextOptions);
            AuthContext.Database.EnsureCreated();
        }
    }
}
