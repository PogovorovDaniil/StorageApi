using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageApi.Helpers;
using StorageApi.Models.DBO.Storage;
using StorageApi.Services;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace StorageApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class QueryController : ControllerBase
    {
        private StorageService _storageService;
        public QueryController(StorageService storageService)
        {
            _storageService = storageService;
        }

        [ActionLogger]
        [ProducesResponseType(typeof(IEnumerable<Store>), StatusCodes.Status200OK)]
        [HttpGet("Store")]
        [HttpGet("Store/{id}")]
        public async Task<IActionResult> GetStore(int? id, [AllowNull] string name)
        {
            IEnumerable<Store> stores;
            if (id.HasValue)
                stores = await _storageService.GetStore(id.Value);
            else if (name is not null)
                stores = await _storageService.GetStore(name);
            else
                stores = await _storageService.GetStores();
            return new JsonResult(stores);
        }

        [ActionLogger]
        [ProducesResponseType(typeof(IEnumerable<Brand>), StatusCodes.Status200OK)]
        [HttpGet("Brand")]
        [HttpGet("Brand/{id}")]
        public async Task<IActionResult> GetBrand(int? id, [AllowNull] string name)
        {
            IEnumerable<Brand> brands;
            if (id.HasValue)
                brands = await _storageService.GetBrand(id.Value);
            else if (name is not null)
                brands = await _storageService.GetBrand(name);
            else
                brands = await _storageService.GetBrands();
            return new JsonResult(brands);
        }
    }
}
