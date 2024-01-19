using MediatR;
using Microsoft.AspNetCore.Mvc;
using StorageApi.Core.Interfaces;
using StorageApi.Core.Models;
using StorageApi.Core.Models.TemplateResult;
using System.Threading.Tasks;

namespace StorageApi.Core.Helpers
{
    public static class MediatorExtention
    {
        public static async Task<IActionResult> Query<TRequest>(this IMediator mediator, TRequest request) where TRequest : IQuery
        {
            return new JsonResult(await mediator.Send(request));
        }

        public static async Task<IActionResult> Execute<TRequest>(this IMediator mediator, TRequest request) where TRequest : ICommand
        {
            CommandResult result = (CommandResult)await mediator.Send(request);

            if (result.Success) return new JsonResult(result.Result);
            return new JsonResult(new ExceptionResult(result.ErrorMessage)) { StatusCode = result.StatusCode };
        }
    }
}
