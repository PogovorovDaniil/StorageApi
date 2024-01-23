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
    public class ProductController : CommandQueryController
    {
        public ProductController(IMediator mediator) : base(mediator) { }

        [ActionLogger]
        [ProducesResponseType(typeof(IEnumerable<Models.Product>), StatusCodes.Status200OK)]
        [HttpGet("Product")]
        public Task<IActionResult> GetProduct([Required] long id)
        {
            return Query(new GetProductQuery
            {
                Id = id
            });
        }

        [ActionLogger]
        [ProducesResponseType(typeof(IEnumerable<Models.Product>), StatusCodes.Status200OK)]
        [HttpGet("ProductByName")]
        public Task<IActionResult> ProductByName(string name)
        {
            return Query(new GetProductQuery
            {
                Name = name
            });
        }

        [ActionLogger]
        [ProducesResponseType(typeof(Models.Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status400BadRequest)]
        [HttpPost("Product")]
        public Task<IActionResult> PostProduct([FromBody] PostProductCommand product)
        {
            return Execute(product);
        }

        [ActionLogger]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status400BadRequest)]
        [HttpDelete("Product")]
        public Task<IActionResult> DeleteProduct([Required] long id)
        {
            return Execute(new DeleteProductQuery
            {
                Id = id
            });
        }
    }
}
