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
    public class PutOfferStockCommand : IRequest<CommandResult<SuccessResult>>, ICommand
    {
        public long OfferId { get; set; }
        public StoreStock[] StoreStocks { get; set; }
        public class StoreStock
        {
            public long StoreId { get; set; }
            public long Quantity { get; set; }
        }

        public class PutOfferStockCommandHandler : IRequestHandler<PutOfferStockCommand, CommandResult<SuccessResult>>
        {
            private readonly IStorageService storageService;
            public PutOfferStockCommandHandler(IStorageService storageService)
            {
                this.storageService = storageService;
            }
            public async Task<CommandResult<SuccessResult>> Handle(PutOfferStockCommand request, CancellationToken cancellationToken)
            {
                DBChangeResult dbResult = await storageService.PutOfferStock(request);
                CommandResult<SuccessResult> result = new CommandResult<SuccessResult> { Success = false };
                switch (dbResult)
                {
                    case DBChangeResult.Success:
                        result.Success = true;
                        result.Result = new SuccessResult("Success");
                        return result;
                    default:
                        result.StatusCode = StatusCodes.Status400BadRequest;
                        result.ErrorMessage = "Unknown error";
                        return result;
                }
            }
        }
    }
}
