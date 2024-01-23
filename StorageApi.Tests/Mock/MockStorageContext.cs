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
            var _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            var _contextOptions = new DbContextOptionsBuilder<StorageContext>().UseSqlite(_connection).Options;
            StorageContext = new StorageContext(_contextOptions);
            StorageContext.Database.EnsureCreated();
        }
    }
}
