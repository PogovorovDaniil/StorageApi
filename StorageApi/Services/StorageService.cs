using Microsoft.EntityFrameworkCore;
using StorageApi.Models;
using StorageApi.Models.APIO.Storage;
using StorageApi.Models.Contexts;
using StorageApi.Models.DBO.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorageApi.Services
{
    public class StorageService
    {
        private readonly StorageContext _context;

        public StorageService(StorageContext context)
        {
            _context = context;
        }
        #region Store
        public async Task<(DBCreateResult result, Store store)> CreateStore(PostStore store)
        {
            if (await _context.Stores.AnyAsync(u => u.Name == store.Name))
            {
                return (DBCreateResult.AlreadyExist, null);
            }
            await _context.Stores.AddAsync(new Store()
            {
                Name = store.Name
            });
            if (await _context.SaveChangesAsync() == 1)
            {
                Store dbStore = await _context.Stores.FirstAsync(s => s.Name == store.Name);
                return (DBCreateResult.Success, dbStore);
            }
            return (DBCreateResult.UnknownError, null);
        }

        public async Task<IEnumerable<Store>> GetStore(int id)
        {
            Store store = await _context.Stores.FirstOrDefaultAsync(x => x.Id == id);
            if (store is null) return Array.Empty<Store>();
            return new[] { store };
        }

        public async Task<IEnumerable<Store>> GetStore(string name)
        {
            return await _context.Stores.FromSqlRaw(
@"SELECT Id, Name
FROM `Stores`
WHERE LOWER(Name) LIKE LOWER({0})", $"%{name}%").ToArrayAsync();
        }

        public async Task<IEnumerable<Store>> GetStores()
        {
            return await _context.Stores.ToArrayAsync();
        }
        #endregion
    }
}
