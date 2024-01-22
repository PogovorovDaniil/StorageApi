using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageApi.Core.Helpers;
using StorageApi.Core.Models.TemplateResult;
using StorageApi.Storage.Requests.Commands;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace StorageApi.Storage.Controllers
{
    [Authorize]
    public class OfferController : ControllerBase
    {
        private readonly IMediator mediator;
        public OfferController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [ActionLogger]
        [ProducesResponseType(typeof(Models.Product.GetProductOffer), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status400BadRequest)]
        [HttpPost("Offer")]
        public Task<IActionResult> PostOffer([FromBody] PostOfferCommand store)
        {
            return mediator.Execute(store);
        }

        [ActionLogger]
        [ProducesResponseType(typeof(Models.Product.GetProductOffer), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status400BadRequest)]
        [HttpDelete("Offer")]
        public Task<IActionResult> DeleteOffer([Required] long id)
        {
            return mediator.Execute(new DeleteOfferQuery
            {
                Id = id
            });
        }
    }
}
