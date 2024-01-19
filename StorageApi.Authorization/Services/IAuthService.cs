using StorageApi.Core.Models.Constants;
using System.Threading.Tasks;

namespace StorageApi.Authorization.Services
{
    public interface IAuthService
    {
        public Task<(bool pass, string role)> TryLogInAsync(string login, string password);

        public Task<DBCreateResult> TryCreateUserAsync(string login, string password);

        public string GetNewToken(string login, string role);
    }
}
