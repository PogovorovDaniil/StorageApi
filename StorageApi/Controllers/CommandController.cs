using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageApi.Helpers;
using StorageApi.Models;
using StorageApi.Models.APIO;
using StorageApi.Models.APIO.Storage;
using StorageApi.Models.DBO.Storage;
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
    }
}
