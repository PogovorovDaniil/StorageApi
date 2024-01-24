using Microsoft.EntityFrameworkCore;
using StorageApi.Core.Models.Constants;
using StorageApi.Database.Contexts;
using StorageApi.Database.Models.Storage;
using StorageApi.Storage.Requests.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageApi.Storage.Services
{
    public class StorageService : IStorageService
    {
        private readonly StorageContext context;

        public StorageService(StorageContext context)
        {
            this.context = context;
        }
        #region Store
        public async Task<(DBCreateResult result, Store store)> CreateStore(PostStoreCommand store)
        {
            if (await context.Stores.AnyAsync(u => u.Name.ToLower().Trim() == store.Name.ToLower().Trim()))
            {
                return (DBCreateResult.AlreadyExist, null);
            }
            Store dbStore = new Store()
            {
                Name = store.Name
            };
            await context.Stores.AddAsync(dbStore);
            if (await context.SaveChangesAsync() == 1)
            {
                return (DBCreateResult.Success, dbStore);
            }
            return (DBCreateResult.UnknownError, null);
        }

        public async Task<IEnumerable<Store>> GetStore(long id)
        {
            Store store = await context.Stores.FindAsync(id);
            if (store is null) return Array.Empty<Store>();
            return new[] { store };
        }

        public async Task<IEnumerable<Store>> GetStore(string name)
        {
            return await context.Stores.FromSqlRaw(
@"SELECT Id, Name
FROM `Stores`
WHERE LOWER(Name) LIKE LOWER({0})", $"%{name}%").ToArrayAsync();
        }

        public async Task<IEnumerable<Store>> GetStores()
        {
            return await context.Stores.ToArrayAsync();
        }
        #endregion

        #region Brand
        public async Task<(DBCreateResult result, Brand store)> CreateBrand(PostBrandCommand brand)
        {
            if (await context.Brands.AnyAsync(u => u.Name.ToLower().Trim() == brand.Name.ToLower().Trim()))
            {
                return (DBCreateResult.AlreadyExist, null);
            }
            Brand dbBrand = new Brand()
            {
                Name = brand.Name
            };
            await context.Brands.AddAsync(dbBrand);
            if (await context.SaveChangesAsync() == 1)
            {
                return (DBCreateResult.Success, dbBrand);
            }
            return (DBCreateResult.UnknownError, null);
        }

        public async Task<IEnumerable<Brand>> GetBrand(long id)
        {
            Brand brand = await context.Brands.FindAsync(id);
            if (brand is null) return Array.Empty<Brand>();
            return new[] { brand };
        }

        public async Task<IEnumerable<Brand>> GetBrand(string name)
        {
            return await context.Brands.FromSqlRaw(
@"SELECT Id, Name
FROM `Brands`
WHERE LOWER(Name) LIKE LOWER({0})", $"%{name}%").ToArrayAsync();
        }

        public async Task<IEnumerable<Brand>> GetBrands()
        {
            return await context.Brands.ToArrayAsync();
        }
        #endregion

        #region Product
        public async Task<(DBCreateResult result, Product dbProduct)> CreateProduct(PostProductCommand product)
        {
            if (await context.Products.AnyAsync(u => u.Name.ToLower().Trim() == product.Name.ToLower().Trim()))
            {
                return (DBCreateResult.AlreadyExist, null);
            }
            Brand dbBrand = await context.Brands.FindAsync(product.BrandId);
            Product dbProduct = new Product()
            {
                Name = product.Name,
                Brand = dbBrand,
            };
            await context.Products.AddAsync(dbProduct);
            if (await context.SaveChangesAsync() == 0) return (DBCreateResult.UnknownError, null);

            foreach (var offer in product.Offers)
            {
                Offer dbOffer = new Offer
                {
                    Product = dbProduct,
                    Color = offer.Color,
                    Size = offer.Size,
                    Price = offer.Price,
                };
                await context.Offers.AddAsync(dbOffer);
            }
            if (await context.SaveChangesAsync() == 0) return (DBCreateResult.UnknownError, null);
            
            return (DBCreateResult.Success, dbProduct);
        }

        public async Task<IEnumerable<Product>> GetProduct(long id)
        {
            Product product = await context.Products
                .Include(p => p.Brand)
                .Include(p => p.Offers)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (product is null) return Array.Empty<Product>();
            return new[] { product };
        }

        public async Task<IEnumerable<Product>> GetProduct(string name)
        {
            return await context.Products.FromSqlRaw(
@"SELECT Id, BrandId, Name
FROM `Products`
WHERE LOWER(Name) LIKE LOWER({0})", $"%{name}%")
                .Include(p => p.Brand)
                .Include(p => p.Offers)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await context.Products
                .Include(p => p.Brand)
                .Include(p => p.Offers)
                .ToArrayAsync();
        }

        public async Task<DBDeleteResult> DeleteProduct(long id)
        {
            Product product = await context.Products.FindAsync(id);
            if (product is null) return DBDeleteResult.UnknownError;
            context.Remove(product);
            if(await context.SaveChangesAsync() == 0) return DBDeleteResult.UnknownError;
            return DBDeleteResult.Success;
        }
        #endregion

        #region Offer
        public async Task<(DBCreateResult dbResult, Offer dbOffer)> CreateOffer(PostOfferCommand offer)
        {
            if (await context.Offers.AnyAsync(o => o.Size == offer.Size && o.Color == offer.Color))
            {
                return (DBCreateResult.AlreadyExist, null);
            }
            Product dbProduct = await context.Products.FindAsync(offer.ProductId);
            if(dbProduct is null) return (DBCreateResult.UnknownError, null);
            Offer dbOffer = new Offer
            {
                Product = dbProduct,
                Color = offer.Color,
                Size = offer.Size,
                Price = offer.Price,
            };

            await context.Offers.AddAsync(dbOffer);
            if (await context.SaveChangesAsync() == 0) return (DBCreateResult.UnknownError, null);

            return (DBCreateResult.Success, dbOffer);
        }

        public async Task<DBDeleteResult> DeleteOffer(long id)
        {
            Offer offer = await context.Offers.FirstOrDefaultAsync(x => x.Id == id);
            if (offer is null) return DBDeleteResult.UnknownError;
            context.Remove(offer);
            if (await context.SaveChangesAsync() == 0) return DBDeleteResult.UnknownError;
            return DBDeleteResult.Success;
        }

        public async Task<DBChangeResult> PutOfferStock(PutOfferStockCommand request)
        {
            Offer offer = context.Offers.Find(request.OfferId);
            foreach (var stock in request.StoreStocks)
            {
                Store store = context.Stores.Find(stock.StoreId);

                var offerStock = context.OfferStocks.Find(request.OfferId, stock.StoreId);
                if (offerStock is null)
                {
                    context.OfferStocks.Add(new OfferStock
                    {
                        Offer = offer,
                        Store = store,
                        Quantity = stock.Quantity
                    });
                    continue;
                }
                offerStock.Quantity = stock.Quantity;
            }
            await context.SaveChangesAsync();
            return DBChangeResult.Success;
        }

        public async Task<OfferStock[]> GetOfferStocks(long offerId)
        {
            return await context.OfferStocks.Include(o => o.Offer).Include(o => o.Store).Where(o => o.Offer.Id == offerId).ToArrayAsync();
        }
        #endregion
    }
}
