using MediatR;
using Microsoft.AspNetCore.Http;
using StorageApi.Core.Interfaces;
using StorageApi.Core.Models;
using StorageApi.Core.Models.Constants;
using StorageApi.Storage.Models;
using StorageApi.Storage.Services;
using System.Threading;
using System.Threading.Tasks;

namespace StorageApi.Storage.Requests.Commands
{
    public class PostOfferCommand : IRequest<CommandResult<Product.GetProductOffer>>, ICommand
    {
        public long ProductId { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }

        public class PostOfferCommandHandler : IRequestHandler<PostOfferCommand, CommandResult<Product.GetProductOffer>>
        {
            private readonly IStorageService storageService;
            public PostOfferCommandHandler(IStorageService storageService)
            {
                this.storageService = storageService;
            }
            public async Task<CommandResult<Product.GetProductOffer>> Handle(PostOfferCommand request, CancellationToken cancellationToken)
            {
                var (dbResult, dbOffer) = await storageService.CreateOffer(request);
                CommandResult<Product.GetProductOffer> result = new CommandResult<Product.GetProductOffer> { Success = false };
                switch (dbResult)
                {
                    case DBCreateResult.Success:
                        result.Success = true;
                        result.Result = new Product.GetProductOffer() { Id = dbOffer.Id, Color = dbOffer.Color, Size = dbOffer.Size, Price = dbOffer.Price };
                        return result;
                    case DBCreateResult.AlreadyExist:
                        result.StatusCode = StatusCodes.Status409Conflict;
                        result.ErrorMessage = "Offer already exist";
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
