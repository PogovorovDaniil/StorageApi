using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StorageApi.Database.Contexts;
using System;

namespace StorageApi.Tests
{
    public class MoqStorageContext
    {
        private StorageContext _storageContext;
        public MoqStorageContext()
        {
            var _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            var _contextOptions = new DbContextOptionsBuilder<StorageContext>().UseSqlite(_connection).Options;
            _storageContext = new StorageContext(_contextOptions);
            _storageContext.Database.EnsureCreated();
        }

        public StorageContext GetContext() { return _storageContext; }
    }
}
