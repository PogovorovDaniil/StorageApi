using MediatR;
using StorageApi.Core.Interfaces;
using StorageApi.Storage.Models;
using StorageApi.Storage.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StorageApi.Storage.Requests.Queries
{
    public class GetProductQuery : IRequest<Product[]>, IQuery
    {
        public long? Id { get; set; }
        public string Name { get; set; }

        public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product[]>
        {
            private readonly IStorageService storageService;
            public GetProductQueryHandler(IStorageService storageService)
            {
                this.storageService = storageService;
            }

            public async Task<Product[]> Handle(GetProductQuery request, CancellationToken cancellationToken)
            {
                IEnumerable<Database.Models.Storage.Product> products;
                if (request.Id.HasValue)
                    products = await storageService.GetProduct(request.Id.Value);
                else if (request.Name is not null)
                    products = await storageService.GetProduct(request.Name);
                else
                    products = await storageService.GetProducts();

                return products.Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    BrandId = p.Brand.Id,
                    BrandName = p.Brand.Name,
                    Offers = p.Offers.Select(o => new Product.GetProductOffer
                    {
                        Id = o.Id,
                        Price = o.Price,
                        Color = o.Color,
                        Size = o.Size,
                    })
                }).ToArray();
            }
        }
    }
}
