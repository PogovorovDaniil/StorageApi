using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageApi.Core.Helpers;
using StorageApi.Core.Models.TemplateResult;
using StorageApi.Storage.Requests.Commands;
using StorageApi.Storage.Requests.Queries;
using System.Threading.Tasks;

namespace StorageApi.Storage.Controllers
{
    [Authorize]
    public class OfferController : CommandQueryController
    {
        public OfferController(IMediator mediator) : base(mediator) { }

        [ActionLogger]
        [ProducesResponseType(typeof(Models.Product.GetProductOffer), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status400BadRequest)]
        [HttpPost("Offer")]
        public Task<IActionResult> PostOffer([FromBody] PostOfferCommand offer)
        {
            return Execute(offer);
        }

        [ActionLogger]
        [ProducesResponseType(typeof(Models.Product.GetProductOffer), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status400BadRequest)]
        [HttpDelete("Offer")]
        public Task<IActionResult> DeleteOffer([FromQuery] DeleteOfferQuery offer)
        {
            return Execute(offer);
        }

        [ActionLogger]
        [ProducesResponseType(typeof(Models.Product.GetProductOffer), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status400BadRequest)]
        [HttpPut("Offer/Stock")]
        public Task<IActionResult> PutOfferStock([FromBody] PutOfferStockCommand offer)
        {
            return Execute(offer);
        }

        [ActionLogger]
        [ProducesResponseType(typeof(Models.Product.GetProductOffer), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status400BadRequest)]
        [HttpGet("Offer/Stock")]
        public Task<IActionResult> GetOfferStock([FromQuery] GetOfferStockQuery offer)
        {
            return Query(offer);
        }
    }
}
