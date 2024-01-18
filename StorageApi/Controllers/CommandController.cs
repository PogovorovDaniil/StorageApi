using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageApi.Database.Models.Storage;
using StorageApi.Helpers;
using StorageApi.Models.Constants;
using StorageApi.Models.Storage;
using StorageApi.Models.TemplateResult;
using StorageApi.Services;
using System.Threading.Tasks;

namespace StorageApi.Controllers
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
    }
}
