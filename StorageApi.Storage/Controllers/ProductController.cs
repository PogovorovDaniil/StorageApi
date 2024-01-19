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
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;
        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [ActionLogger]
        [ProducesResponseType(typeof(IEnumerable<Models.Product>), StatusCodes.Status200OK)]
        [HttpGet("Product")]
        public Task<IActionResult> GetProduct([Required] long id)
        {
            return mediator.Query(new GetProductQuery
            {
                Id = id
            });
        }

        [ActionLogger]
        [ProducesResponseType(typeof(IEnumerable<Models.Product>), StatusCodes.Status200OK)]
        [HttpGet("ProductByName")]
        public Task<IActionResult> ProductByName(string name)
        {
            return mediator.Query(new GetProductQuery
            {
                Name = name
            });
        }

        [ActionLogger]
        [ProducesResponseType(typeof(Models.Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status400BadRequest)]
        [HttpPost("Product")]
        public Task<IActionResult> PostProduct(PostProductCommand store)
        {
            return mediator.Execute(store);
        }
    }
}
