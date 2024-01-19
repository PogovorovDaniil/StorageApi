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
    public class GetStoreQuery : IRequest<Store[]>, IQuery
    {
        public long? Id { get; set; }
        public string Name { get; set; }

        public class GetStoreQueryHandler : IRequestHandler<GetStoreQuery, Store[]>
        {
            private readonly IStorageService storageService;
            public GetStoreQueryHandler(IStorageService storageService)
            {
                this.storageService = storageService;
            }

            public async Task<Store[]> Handle(GetStoreQuery request, CancellationToken cancellationToken)
            {
                IEnumerable<Database.Models.Storage.Store> stores;
                if (request.Id.HasValue)
                    stores = await storageService.GetStore(request.Id.Value);
                else if (request.Name is not null)
                    stores = await storageService.GetStore(request.Name);
                else
                    stores = await storageService.GetStores();

                return stores.Select(s => new Store
                {
                    Id = s.Id,
                    Name = s.Name,
                }).ToArray();
            }
        }
    }
}
