using MediatR;
using StorageApi.Core.Interfaces;
using StorageApi.Storage.Models;
using StorageApi.Storage.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StorageApi.Storage.Requests.Queries
{
    public class GetOfferStockQuery : IRequest<StoreStock[]>, IQuery
    {
        public long OfferId { get; set; }

        public class GetOfferStockQueryHandler : IRequestHandler<GetOfferStockQuery, StoreStock[]>
        {
            private readonly IStorageService storageService;
            public GetOfferStockQueryHandler(IStorageService storageService)
            {
                this.storageService = storageService;
            }

            public async Task<StoreStock[]> Handle(GetOfferStockQuery request, CancellationToken cancellationToken)
            {
                var stocks = await storageService.GetOfferStocks(request.OfferId);

                return stocks.Select(s => new StoreStock
                {
                    StoreId = s.Store.Id,
                    Quantity = s.Quantity,
                }).ToArray();
            }
        }
    }
}
