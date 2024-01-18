using Microsoft.EntityFrameworkCore;
using StorageApi.Core.Models.Constants;
using StorageApi.Database.Contexts;
using StorageApi.Database.Models.Storage;
using StorageApi.Storage.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorageApi.Storage.Services
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
            if (await _context.Stores.AnyAsync(u => u.Name.ToLower().Trim() == store.Name.ToLower().Trim()))
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

        #region Brand
        public async Task<(DBCreateResult result, Brand store)> CreateBrand(PostBrand brand)
        {
            if (await _context.Brands.AnyAsync(u => u.Name.ToLower().Trim() == brand.Name.ToLower().Trim()))
            {
                return (DBCreateResult.AlreadyExist, null);
            }
            await _context.Brands.AddAsync(new Brand()
            {
                Name = brand.Name
            });
            if (await _context.SaveChangesAsync() == 1)
            {
                Brand dbBrand = await _context.Brands.FirstAsync(s => s.Name == brand.Name);
                return (DBCreateResult.Success, dbBrand);
            }
            return (DBCreateResult.UnknownError, null);
        }

        public async Task<IEnumerable<Brand>> GetBrand(int id)
        {
            Brand brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == id);
            if (brand is null) return Array.Empty<Brand>();
            return new[] { brand };
        }

        public async Task<IEnumerable<Brand>> GetBrand(string name)
        {
            return await _context.Brands.FromSqlRaw(
@"SELECT Id, Name
FROM `Brands`
WHERE LOWER(Name) LIKE LOWER({0})", $"%{name}%").ToArrayAsync();
        }

        public async Task<IEnumerable<Brand>> GetBrands()
        {
            return await _context.Brands.ToArrayAsync();
        }
        #endregion

        #region Product
        public Task<(DBCreateResult result, Product dbProduct)> CreateProduct(PostProduct product)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
