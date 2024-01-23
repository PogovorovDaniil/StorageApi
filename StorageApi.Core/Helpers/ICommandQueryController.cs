using MediatR;
using Microsoft.AspNetCore.Mvc;
using StorageApi.Core.Interfaces;
using StorageApi.Core.Models.TemplateResult;
using StorageApi.Core.Models;
using System.Threading.Tasks;

namespace StorageApi.Core.Helpers
{
    public abstract class CommandQueryController : ControllerBase
    {
        private IMediator mediator { get; set; }
        protected CommandQueryController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        protected async Task<IActionResult> Query<TRequest>(TRequest request) where TRequest : IQuery
        {
            return new JsonResult(await mediator.Send(request));
        }

        protected async Task<IActionResult> Execute<TRequest>(TRequest request) where TRequest : ICommand
        {
            CommandResult result = (CommandResult)await mediator.Send(request);

            if (result.Success) return new JsonResult(result.Result);
            return new JsonResult(new ExceptionResult(result.ErrorMessage)) { StatusCode = result.StatusCode };
        }
    }
}
