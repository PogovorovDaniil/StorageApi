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
    public class PostStoreCommand : IRequest<CommandResult<Store>>, ICommand
    {
        public string Name { get; set; }

        public class PostStoreCommandHandler : IRequestHandler<PostStoreCommand, CommandResult<Store>>
        {
            private readonly IStorageService storageService;
            public PostStoreCommandHandler(IStorageService storageService)
            {
                this.storageService = storageService;
            }
            public async Task<CommandResult<Store>> Handle(PostStoreCommand request, CancellationToken cancellationToken)
            {
                var (dbResult, dbStore) = await storageService.CreateStore(request);
                CommandResult<Store> result = new CommandResult<Store> { Success = false };
                switch (dbResult)
                {
                    case DBCreateResult.Success:
                        result.Success = true;
                        result.Result = new Store() { Id = dbStore.Id, Name = dbStore.Name };
                        return result;
                    case DBCreateResult.AlreadyExist:
                        result.StatusCode = StatusCodes.Status409Conflict;
                        result.ErrorMessage = "Store already exist";
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
