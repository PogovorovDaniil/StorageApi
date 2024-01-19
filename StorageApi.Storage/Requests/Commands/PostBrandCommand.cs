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
    public class PostBrandCommand : IRequest<CommandResult<Brand>>, ICommand
    {
        public string Name { get; set; }

        public class PostBrandCommandHandler : IRequestHandler<PostBrandCommand, CommandResult<Brand>>
        {
            private readonly IStorageService storageService;
            public PostBrandCommandHandler(IStorageService storageService)
            {
                this.storageService = storageService;
            }
            public async Task<CommandResult<Brand>> Handle(PostBrandCommand request, CancellationToken cancellationToken)
            {
                var (dbResult, dbBrand) = await storageService.CreateBrand(request);
                CommandResult<Brand> result = new CommandResult<Brand> { Success = false };
                switch (dbResult)
                {
                    case DBCreateResult.Success:
                        result.Success = true;
                        result.Result = new Brand() { Id = dbBrand.Id, Name = dbBrand.Name };
                        return result;
                    case DBCreateResult.AlreadyExist:
                        result.StatusCode = StatusCodes.Status409Conflict;
                        result.ErrorMessage = "Brand already exist";
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
