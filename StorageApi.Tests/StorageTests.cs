using StorageApi.Core.Models.Constants;
using StorageApi.Database.Models.Storage;
using StorageApi.Storage.Requests.Commands;
using StorageApi.Storage.Services;
using System.Linq;

namespace StorageApi.Tests
{
    public class StorageTests
    {
        private IStorageService storageService;

        private readonly Store storeDNS;
        private readonly Store storeMVideo;
        private readonly Store storeSitilink;

        private readonly Brand brandApple;
        private readonly Brand brandMicrosoft;
        private readonly Brand brandSamsung;

        private readonly Product productSamsung;
        private readonly Product productApple;

        public StorageTests() 
        {
            var moqContext = new MockStorageContext();
            var storageContext = moqContext.StorageContext; 
            storageService = new StorageService(storageContext);

            storeDNS = new Store() { Name = "DNS" };
            storeMVideo = new Store() { Name = "М.Видео" };
            storeSitilink = new Store() { Name = "Ситилинк" };

            storageContext.Stores.Add(storeDNS);
            storageContext.Stores.Add(storeMVideo);
            storageContext.Stores.Add(storeSitilink);

            brandApple = new Brand() { Name = "Apple" };
            brandMicrosoft = new Brand() { Name = "Microsoft" };
            brandSamsung = new Brand() { Name = "Samsung" };

            storageContext.Brands.Add(brandApple);
            storageContext.Brands.Add(brandMicrosoft);
            storageContext.Brands.Add(brandSamsung);

            storageContext.SaveChanges();

            productSamsung = new Product() { Brand = brandSamsung, Name = "Samsung Galaxy Watch 5" };
            storageContext.Products.Add(productSamsung);
            storageContext.SaveChanges();

            storageContext.Offers.Add(new Offer() { Product = productSamsung, Color = "black", Price = 21999 });
            storageContext.Offers.Add(new Offer() { Product = productSamsung, Color = "white", Price = 20999 });
            storageContext.SaveChanges();

            productApple = new Product() { Brand = brandApple, Name = "Apple iPhone 15 128Gb Black" };
            storageContext.Products.Add(productApple);
            storageContext.SaveChanges();

            storageContext.Offers.Add(new Offer() { Product = productApple, Color = "black", Price = 93490 });
            storageContext.SaveChanges();

            
        }

        #region Store
        [Fact]
        public async void GetStores()
        {
            var stores = await storageService.GetStores();
            Assert.NotEmpty(stores);
        }

        [Theory]
        [InlineData(1, "DNS")]
        [InlineData(2, "М.Видео")]
        public async void GetStore(long id, string name)
        {
            var store1 = (await storageService.GetStore(id)).First();
            Assert.Equal(name, store1.Name);
            var store2 = (await storageService.GetStore(name)).First();
            Assert.Equal(id, store2.Id);
        }

        [Fact]
        public async void CreateStore()
        {
            var (result, store) = await storageService.CreateStore(new PostStoreCommand
            {
                Name = "Вольтмарт",
            });
            Assert.Equal(DBCreateResult.Success, result);
            Assert.NotNull(store);
        }
        #endregion

        #region Brand
        [Fact]
        public async void GetBrands()
        {
            var brands = await storageService.GetBrands();
            Assert.NotEmpty(brands);
        }

        [Theory]
        [InlineData(1, "Apple")]
        [InlineData(2, "Microsoft")]
        public async void GetBrand(long id, string name)
        {
            var brand1 = (await storageService.GetBrand(id)).First();
            Assert.Equal(name, brand1.Name);
            var brand2 = (await storageService.GetBrand(name)).First();
            Assert.Equal(id, brand2.Id);
        }

        [Fact]
        public async void CreateBrand()
        {
            var (result, brand) = await storageService.CreateBrand(new PostBrandCommand
            {
                Name = "Вольтмарт",
            });
            Assert.Equal(DBCreateResult.Success, result);
            Assert.NotNull(brand);
        }
        #endregion

        #region Product
        [Fact]
        public async void GetProducts()
        {
            var products = await storageService.GetProducts();
            Assert.NotEmpty(products);
        }

        [Theory]
        [InlineData(1, "Samsung Galaxy Watch 5")]
        [InlineData(2, "Apple iPhone 15 128Gb Black")]
        public async void GetProduct(long id, string name)
        {
            var product1 = (await storageService.GetProduct(id)).First();
            Assert.Equal(name, product1.Name);
            var product2 = (await storageService.GetProduct(name)).First();
            Assert.Equal(id, product2.Id);
        }

        [Fact]
        public async void CreateProduct()
        {
            var (result, product) = await storageService.CreateProduct(new PostProductCommand
            {
                BrandId = 1,
                Name = "Apple Macbook Pro 14 Late",
                Offers = new PostProductCommand.PostProductOffer[]
                {
                    new()
                    {
                        Color = "gray",
                        Price = 144_073
                    }
                }
            });
            Assert.Equal(DBCreateResult.Success, result);
            Assert.NotNull(product);
        }

        [Fact]
        public async void DeleteProduct()
        {
            var productId = productSamsung.Id;
            var result = await storageService.DeleteProduct(productId);
            Assert.Equal(DBDeleteResult.Success, result);

            var product = await storageService.GetProduct(productId);
            Assert.Empty(product);
        }
        #endregion

        #region OfferStock

        [Fact]
        public async void AddOfferStock()
        {
            DBChangeResult result = await storageService.PutOfferStock(new PutOfferStockCommand
            {
                OfferId = productApple.Offers.First().Id,
                StoreStocks = new PutOfferStockCommand.StoreStock[]
                {
                    new()
                    {
                        Quantity = 1,
                        StoreId = storeDNS.Id,
                    },
                    new()
                    {
                        Quantity = 0,
                        StoreId = storeMVideo.Id,
                    },
                    new()
                    {
                        Quantity = 10,
                        StoreId = storeSitilink.Id,
                    }
                }
            });
            Assert.Equal(DBChangeResult.Success, result);

            var stocks = await storageService.GetOfferStocks(productApple.Offers.First().Id);
            Assert.Equal(1, stocks.First(x => x.Store.Id == storeDNS.Id).Quantity);
            Assert.Equal(0, stocks.First(x => x.Store.Id == storeMVideo.Id).Quantity);
            Assert.Equal(10, stocks.First(x => x.Store.Id == storeSitilink.Id).Quantity);
        }

        #endregion
    }
}
