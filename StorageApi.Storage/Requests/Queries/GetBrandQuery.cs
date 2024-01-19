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
    public class GetBrandQuery : IRequest<Brand[]>, IQuery
    {
        public long? Id { get; set; }
        public string Name { get; set; }

        public class GetBrandQueryHandler : IRequestHandler<GetBrandQuery, Brand[]>
        {
            private readonly IStorageService storageService;
            public GetBrandQueryHandler(IStorageService storageService)
            {
                this.storageService = storageService;
            }

            public async Task<Brand[]> Handle(GetBrandQuery request, CancellationToken cancellationToken)
            {
                IEnumerable<Database.Models.Storage.Brand> stores;
                if (request.Id.HasValue)
                    stores = await storageService.GetBrand(request.Id.Value);
                else if (request.Name is not null)
                    stores = await storageService.GetBrand(request.Name);
                else
                    stores = await storageService.GetBrands();

                return stores.Select(s => new Brand
                {
                    Id = s.Id,
                    Name = s.Name,
                }).ToArray();
            }
        }
    }
}
