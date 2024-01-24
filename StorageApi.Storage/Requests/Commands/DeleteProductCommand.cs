using MediatR;
using Microsoft.AspNetCore.Http;
using StorageApi.Core.Interfaces;
using StorageApi.Core.Models;
using StorageApi.Core.Models.Constants;
using StorageApi.Core.Models.TemplateResult;
using StorageApi.Storage.Services;
using System.Threading;
using System.Threading.Tasks;

namespace StorageApi.Storage.Requests.Commands
{
    public class DeleteProductCommand : IRequest<CommandResult<SuccessResult>>, ICommand
    {
        public long Id { get; set; }

        public class DeleteProductQueryHandler : IRequestHandler<DeleteProductCommand, CommandResult<SuccessResult>>
        {
            private readonly IStorageService storageService;
            public DeleteProductQueryHandler(IStorageService storageService)
            {
                this.storageService = storageService;
            }

            public async Task<CommandResult<SuccessResult>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                var result = await storageService.DeleteProduct(request.Id) == DBDeleteResult.Success;
                return new CommandResult<SuccessResult>
                {
                    Result = new SuccessResult("Success"),
                    Success = result,
                    ErrorMessage = "Delete failed",
                    StatusCode = result ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest
                };
            }
        }
    }
}
