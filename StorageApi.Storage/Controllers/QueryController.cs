using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageApi.Core.Helpers;
using StorageApi.Database.Models.Storage;
using StorageApi.Storage.Models;
using StorageApi.Storage.Services;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace StorageApi.Storage.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class QueryController : ControllerBase
    {
        private IStorageService _storageService;
        public QueryController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [ActionLogger]
        [ProducesResponseType(typeof(IEnumerable<GetStore>), StatusCodes.Status200OK)]
        [HttpGet("Store")]
        [HttpGet("Store/{id}")]
        public async Task<IActionResult> GetStore(long? id, [AllowNull] string name)
        {
            IEnumerable<Store> stores;
            if (id.HasValue)
                stores = await _storageService.GetStore(id.Value);
            else if (name is not null)
                stores = await _storageService.GetStore(name);
            else
                stores = await _storageService.GetStores();

            GetStore[] result = stores.Select(s => new GetStore 
            {
                Id = s.Id,
                Name = s.Name,
            }).ToArray();

            return new JsonResult(result);
        }

        [ActionLogger]
        [ProducesResponseType(typeof(IEnumerable<GetBrand>), StatusCodes.Status200OK)]
        [HttpGet("Brand")]
        [HttpGet("Brand/{id}")]
        public async Task<IActionResult> GetBrand(long? id, [AllowNull] string name)
        {
            IEnumerable<Brand> brands;
            if (id.HasValue)
                brands = await _storageService.GetBrand(id.Value);
            else if (name is not null)
                brands = await _storageService.GetBrand(name);
            else
                brands = await _storageService.GetBrands();

            GetBrand[] result = brands.Select(b => new GetBrand
            {
                Id = b.Id,
                Name = b.Name,
            }).ToArray();

            return new JsonResult(result);
        }

        [ActionLogger]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [HttpGet("Product")]
        [HttpGet("Product/{id}")]
        public async Task<IActionResult> GetProduct(long? id, [AllowNull] string name)
        {
            IEnumerable<Product> brands;
            if (id.HasValue)
                brands = await _storageService.GetProduct(id.Value);
            else if (name is not null)
                brands = await _storageService.GetProduct(name);
            else
                brands = await _storageService.GetProducts();

            GetProduct[] result = brands.Select(p => new GetProduct
            {
                Id = p.Id,
                Name = p.Name,
                BrandId = p.Brand.Id,
                BrandName = p.Brand.Name,
                Offers = p.Offers.Select(o => new GetProduct.GetProductOffer 
                {
                    Id = o.Id,
                    Price = o.Price,
                    Color = o.Color,
                    Size = o.Size,
                })
            }).ToArray();

            return new JsonResult(result);
        }
    }
}
