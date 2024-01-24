using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StorageApi.Database.Contexts;

namespace StorageApi.Tests.Mock
{
    public class MockStorageContext
    {
        public StorageContext StorageContext { get; }
        public MockStorageContext()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var contextOptions = new DbContextOptionsBuilder<StorageContext>().UseSqlite(connection).Options;
            StorageContext = new StorageContext(contextOptions);
            StorageContext.Database.EnsureCreated();
        }
    }
}
