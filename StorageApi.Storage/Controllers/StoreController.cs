using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageApi.Core.Helpers;
using StorageApi.Core.Models.TemplateResult;
using StorageApi.Storage.Requests.Commands;
using StorageApi.Storage.Requests.Queries;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace StorageApi.Storage.Controllers
{
    [Authorize]
    public class StoreController : ControllerBase
    {
        private readonly IMediator mediator;
        public StoreController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [ActionLogger]
        [ProducesResponseType(typeof(IEnumerable<Models.Store>), StatusCodes.Status200OK)]
        [HttpGet("Store")]
        public Task<IActionResult> GetStore([Required] long id)
        {
            return mediator.Query(new GetStoreQuery
            {
                Id = id
            });
        }

        [ActionLogger]
        [ProducesResponseType(typeof(IEnumerable<Models.Store>), StatusCodes.Status200OK)]
        [HttpGet("StoreByName")]
        public Task<IActionResult> StoreByName(string name)
        {
            return mediator.Query(new GetStoreQuery
            {
                Name = name
            });
        }

        [ActionLogger]
        [ProducesResponseType(typeof(Models.Store), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status400BadRequest)]
        [HttpPost("Store")]
        public Task<IActionResult> PostStore(PostStoreCommand store)
        {
            return mediator.Execute(store);
        }
    }
}
