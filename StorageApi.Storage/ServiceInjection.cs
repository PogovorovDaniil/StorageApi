using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StorageApi.Storage.Services;
using System.Reflection;

namespace StorageApi.Storage
{
    public static class ServiceInjection
    {
        public static void AddStorageServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IStorageService, StorageService>(); 
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.Lifetime = ServiceLifetime.Scoped;
            });
        }
    }
}
