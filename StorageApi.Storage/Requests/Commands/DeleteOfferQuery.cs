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
    public class DeleteOfferQuery : IRequest<CommandResult<SuccessResult>>, ICommand
    {
        public long Id { get; set; }

        public class DeleteOfferQueryHandler : IRequestHandler<DeleteOfferQuery, CommandResult<SuccessResult>>
        {
            private readonly IStorageService storageService;
            public DeleteOfferQueryHandler(IStorageService storageService)
            {
                this.storageService = storageService;
            }

            public async Task<CommandResult<SuccessResult>> Handle(DeleteOfferQuery request, CancellationToken cancellationToken)
            {
                var result = await storageService.DeleteOffer(request.Id) == DBDeleteResult.Success;
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
