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
    public class BrandController : CommandQueryController
    {
        public BrandController(IMediator mediator) : base(mediator) { }

        [ActionLogger]
        [ProducesResponseType(typeof(IEnumerable<Models.Brand>), StatusCodes.Status200OK)]
        [HttpGet("Brand")]
        public Task<IActionResult> GetBrand([Required] long id)
        {
            return Query(new GetBrandQuery
            {
                Id = id
            });
        }

        [ActionLogger]
        [ProducesResponseType(typeof(IEnumerable<Models.Brand>), StatusCodes.Status200OK)]
        [HttpGet("BrandByName")]
        public Task<IActionResult> BrandByName(string name)
        {
            return Query(new GetBrandQuery
            {
                Name = name
            });
        }

        [ActionLogger]
        [ProducesResponseType(typeof(Models.Brand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status400BadRequest)]
        [HttpPost("Brand")]
        public Task<IActionResult> PostBrand(PostBrandCommand store)
        {
            return Execute(store);
        }
    }
}
