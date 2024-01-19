using Microsoft.EntityFrameworkCore;
using StorageApi.Core.Models.Constants;
using StorageApi.Database.Contexts;
using StorageApi.Database.Models.Storage;
using StorageApi.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Store dbStore = new Store()
            {
                Name = store.Name
            };
            await _context.Stores.AddAsync(dbStore);
            if (await _context.SaveChangesAsync() == 1)
            {
                return (DBCreateResult.Success, dbStore);
            }
            return (DBCreateResult.UnknownError, null);
        }

        public async Task<IEnumerable<Store>> GetStore(long id)
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
            Brand dbBrand = new Brand()
            {
                Name = brand.Name
            };
            await _context.Brands.AddAsync(dbBrand);
            if (await _context.SaveChangesAsync() == 1)
            {
                return (DBCreateResult.Success, dbBrand);
            }
            return (DBCreateResult.UnknownError, null);
        }

        public async Task<IEnumerable<Brand>> GetBrand(long id)
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
        public async Task<(DBCreateResult result, Product dbProduct)> CreateProduct(PostProduct product)
        {
            if (await _context.Products.AnyAsync(u => u.Name.ToLower().Trim() == product.Name.ToLower().Trim()))
            {
                return (DBCreateResult.AlreadyExist, null);
            }
            Brand dbBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == product.BrandId);
            Product dbProduct = new Product()
            {
                Name = product.Name,
                Brand = dbBrand,
            };
            await _context.Products.AddAsync(dbProduct);
            if (await _context.SaveChangesAsync() == 0) return (DBCreateResult.UnknownError, null);

            foreach (var offer in product.Offers)
            {
                Offer dbOffer = new Offer
                {
                    Product = dbProduct,
                    Color = offer.Color,
                    Size = offer.Size,
                    Price = offer.Price,
                };
                await _context.Offers.AddAsync(dbOffer);
            }
            if (await _context.SaveChangesAsync() == 0) return (DBCreateResult.UnknownError, null);
            
            return (DBCreateResult.Success, dbProduct);
        }

        public async Task<IEnumerable<Product>> GetProduct(long id)
        {
            Product product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Offers)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (product is null) return Array.Empty<Product>();
            return new[] { product };
        }

        public async Task<IEnumerable<Product>> GetProduct(string name)
        {
            return await _context.Products.FromSqlRaw(
@"SELECT Id, BrandId, Name
FROM `Products`
WHERE LOWER(Name) LIKE LOWER({0})", $"%{name}%")
                .Include(p => p.Brand)
                .Include(p => p.Offers)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Offers)
                .ToArrayAsync();
        }
        #endregion
    }
}
