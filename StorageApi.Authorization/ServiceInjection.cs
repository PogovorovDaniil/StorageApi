using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StorageApi.Authorization.Models;
using StorageApi.Authorization.Services;

namespace StorageApi.Authorization
{
    public static class ServiceInjection
    {
        public static void AddAuthServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<AuthConfiguration>();
            builder.Services.AddTransient<IAuthService, AuthService>();
        }
    }
}
