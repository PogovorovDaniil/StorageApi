using StorageApi.Database.Contexts;
using StorageApi.Database.Models.Storage;
using StorageApi.Storage.Services;
using System.Linq;

namespace StorageApi.Tests
{
    public class StorageTests
    {
        MoqStorageContext moqContext;
        StorageContext storageContext => moqContext.GetContext();
        IStorageService storageService;

        public StorageTests() 
        {
            Store storeDNS = new Store() { Name = "DNS" };
            Store storeMVideo = new Store() { Name = "М.Видео" };
            Store storeSitilink = new Store() { Name = "Ситилинк" };
            moqContext = new MoqStorageContext();

            storageContext.Stores.Add(storeDNS);
            storageContext.Stores.Add(storeMVideo);
            storageContext.Stores.Add(storeSitilink);

            Brand brandApple = new Brand() { Name = "Apple" };
            Brand brandMicrosoft = new Brand() { Name = "Microsoft" };
            Brand brandSamsung = new Brand() { Name = "Samsung" };
            storageContext.Brands.Add(brandApple);
            storageContext.Brands.Add(brandMicrosoft);
            storageContext.Brands.Add(brandSamsung);

            storageContext.SaveChanges();

            Product productSamsung = new Product() { Brand = brandSamsung, Name = "Samsung Galaxy Watch 5" };
            storageContext.Products.Add(productSamsung);
            storageContext.SaveChanges();

            storageContext.Offers.Add(new Offer() { Product = productSamsung, Color = "black", Price = 21999 });
            storageContext.Offers.Add(new Offer() { Product = productSamsung, Color = "white", Price = 20999 });
            storageContext.SaveChanges();

            Product productApple = new Product() { Brand = brandApple, Name = "Apple iPhone 15 128Gb Black" };
            storageContext.Products.Add(productApple);
            storageContext.SaveChanges();

            storageContext.Offers.Add(new Offer() { Product = productApple, Color = "black", Price = 93490 });
            storageContext.SaveChanges();

            storageService = new StorageService(storageContext);
        }

        [Fact]
        public async void GetProducts()
        {
            var products = await storageService.GetProducts();
            Assert.Equal(2, products.Count());
        }

        [Theory]
        [InlineData(1, "Samsung Galaxy Watch 5")]
        [InlineData(2, "Apple iPhone 15 128Gb Black")]
        public async void GetProduct(long id, string name)
        {
            var product = (await storageService.GetProduct(id)).First();
            Assert.Equal(name, product.Name);
        }
    }
}
