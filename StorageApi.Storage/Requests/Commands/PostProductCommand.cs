using MediatR;
using Microsoft.AspNetCore.Http;
using StorageApi.Core.Interfaces;
using StorageApi.Core.Models;
using StorageApi.Core.Models.Constants;
using StorageApi.Storage.Models;
using StorageApi.Storage.Services;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StorageApi.Storage.Requests.Commands
{
    public class PostProductCommand : IRequest<CommandResult<Product>>, ICommand
    {
        public string Name { get; set; }
        public long BrandId { get; set; }
        public IEnumerable<PostProductOffer> Offers { get; set; }
        public class PostProductOffer
        {
            [AllowNull]
            public string Color { get; set; }
            [AllowNull]
            public string Size { get; set; }
            public decimal Price { get; set; }
        }

        public class PostProductCommandHandler : IRequestHandler<PostProductCommand, CommandResult<Product>>
        {
            private readonly IStorageService storageService;
            public PostProductCommandHandler(IStorageService storageService)
            {
                this.storageService = storageService;
            }
            public async Task<CommandResult<Product>> Handle(PostProductCommand request, CancellationToken cancellationToken)
            {
                var (dbResult, dbProduct) = await storageService.CreateProduct(request);
                CommandResult<Product> result = new CommandResult<Product> { Success = false };
                switch (dbResult)
                {
                    case DBCreateResult.Success:
                        result.Success = true;
                        result.Result = new Product()
                        {
                            Id = dbProduct.Id,
                            Name = dbProduct.Name,
                            BrandId = dbProduct.Brand.Id,
                            BrandName = dbProduct.Brand.Name,
                            Offers = dbProduct.Offers.Select(dbOffer => new Product.GetProductOffer
                            {
                                Id = dbOffer.Id,
                                Price = dbOffer.Price,
                                Color = dbOffer.Color,
                                Size = dbOffer.Size
                            }).ToList()
                        };
                        return result;
                    case DBCreateResult.AlreadyExist:
                        result.StatusCode = StatusCodes.Status409Conflict;
                        result.ErrorMessage = "Product already exist";
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
