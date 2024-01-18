using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StorageApi.Database.Contexts;

namespace StorageApi.Database
{
    public static class DataBaseInjection
    {
        public static void AddDbContexts(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AuthorizationContext>(
                optionAction => optionAction.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                MariaDbServerVersion.LatestSupportedServerVersion));

            builder.Services.AddDbContext<StorageContext>(
                optionAction => optionAction.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                MariaDbServerVersion.LatestSupportedServerVersion));
        }
    }
}
