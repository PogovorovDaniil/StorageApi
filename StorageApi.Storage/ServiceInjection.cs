using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StorageApi.Storage.Services;

namespace StorageApi.Storage
{
    public static class ServiceInjection
    {
        public static void AddStorageServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<StorageService>();
        }
    }
}
