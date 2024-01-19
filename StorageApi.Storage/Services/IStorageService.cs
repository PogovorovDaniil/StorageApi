using StorageApi.Core.Models.Constants;
using StorageApi.Database.Models.Storage;
using StorageApi.Storage.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorageApi.Storage.Services
{
    public interface IStorageService
    {

        #region Store
        public Task<(DBCreateResult result, Store store)> CreateStore(PostStore store);

        public Task<IEnumerable<Store>> GetStore(long id);

        public Task<IEnumerable<Store>> GetStore(string name);

        public Task<IEnumerable<Store>> GetStores();
        #endregion

        #region Brand
        public Task<(DBCreateResult result, Brand store)> CreateBrand(PostBrand brand);

        public Task<IEnumerable<Brand>> GetBrand(long id);

        public Task<IEnumerable<Brand>> GetBrand(string name);

        public Task<IEnumerable<Brand>> GetBrands();
        #endregion

        #region Product
        public Task<(DBCreateResult result, Product dbProduct)> CreateProduct(PostProduct product);

        public Task<IEnumerable<Product>> GetProduct(long id);

        public Task<IEnumerable<Product>> GetProduct(string name);

        public Task<IEnumerable<Product>> GetProducts();
        #endregion
    }
}
