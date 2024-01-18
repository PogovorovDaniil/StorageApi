using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageApi.Core.Helpers;
using StorageApi.Core.Models.Constants;
using StorageApi.Core.Models.TemplateResult;
using StorageApi.Database.Models.Storage;
using StorageApi.Storage.Models;
using StorageApi.Storage.Services;
using System.Linq;
using System.Threading.Tasks;

namespace StorageApi.Storage.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CommandController : ControllerBase
    {
        private StorageService _storageService;
        public CommandController(StorageService storageService)
        {
            _storageService = storageService;
        }

        [ActionLogger]
        [ProducesResponseType(typeof(GetStore), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status400BadRequest)]
        [HttpPost("Store")]
        public async Task<IActionResult> PostStore(PostStore store)
        {
            (DBCreateResult result, Store dbStore) = await _storageService.CreateStore(store);
            switch (result)
            {
                case DBCreateResult.Success:
                    return new JsonResult(new GetStore() { Id = dbStore.Id, Name = dbStore.Name });
                case DBCreateResult.AlreadyExist:
                    return new JsonResult(new ExceptionResult("Store already exist")) { StatusCode = StatusCodes.Status409Conflict };
                default:
                    return new JsonResult(new ExceptionResult("Unknown error")) { StatusCode = StatusCodes.Status400BadRequest };
            }
        }

        [ActionLogger]
        [ProducesResponseType(typeof(GetBrand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status400BadRequest)]
        [HttpPost("Brand")]
        public async Task<IActionResult> PostBrand(PostBrand brand)
        {
            (DBCreateResult result, Brand dbBrand) = await _storageService.CreateBrand(brand);
            switch (result)
            {
                case DBCreateResult.Success:
                    return new JsonResult(new GetBrand() { Id = dbBrand.Id, Name = dbBrand.Name });
                case DBCreateResult.AlreadyExist:
                    return new JsonResult(new ExceptionResult("Brand already exist")) { StatusCode = StatusCodes.Status409Conflict };
                default:
                    return new JsonResult(new ExceptionResult("Unknown error")) { StatusCode = StatusCodes.Status400BadRequest };
            }
        }

        [ActionLogger]
        [ProducesResponseType(typeof(GetProduct), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status400BadRequest)]
        [HttpPost("Brand")]
        public async Task<IActionResult> PostProduct(PostProduct product)
        {
            (DBCreateResult result, Product dbProduct) = await _storageService.CreateProduct(product);
            switch (result)
            {
                case DBCreateResult.Success:
                    return new JsonResult(new GetProduct()
                    {
                        Id = dbProduct.Id, 
                        Name = dbProduct.Name,
                        BrandId = dbProduct.Brand.Id,
                        BrandName = dbProduct.Brand.Name,
                        Offers = dbProduct.Offers.Select(dbOffer => new GetProduct.Offer
                        {
                            Id = dbOffer.Id,
                            Price = dbOffer.Price,
                            Color = dbOffer.Color,
                            Size = dbOffer.Size
                        }).ToList()
                    });
                case DBCreateResult.AlreadyExist:
                    return new JsonResult(new ExceptionResult("Product already exist")) { StatusCode = StatusCodes.Status409Conflict };
                default:
                    return new JsonResult(new ExceptionResult("Unknown error")) { StatusCode = StatusCodes.Status400BadRequest };
            }
        }
    }
}
