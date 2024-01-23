using Microsoft.Extensions.Configuration;
using StorageApi.Authorization.Models;
using StorageApi.Authorization.Services;
using StorageApi.Core.Models.Constants;
using System.Collections.Generic;

namespace StorageApi.Tests
{
    public class AuthTests
    {
        private IAuthService authService;

        private const string rootLogin = "root";
        private const string rootPassword = "12345676";

        public AuthTests() 
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"Auth:Issuer", "StorageServer"},
                {"Auth:Audience", "StorageClient"},
                {"Auth:IssuerSigningKey", "50180ecc3f2244fb9be595ca5e69fc8d"},
                {"Auth:RootPassword", rootPassword},
            };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var moqContext = new MockAuthContext();
            var authContext = moqContext.AuthContext;
            authService = new AuthService(new AuthConfiguration(configuration), authContext);
        }

        [Fact]
        public async void AuthRoot()
        {
            var (pass, role) = await authService.TryLogInAsync(rootLogin, rootPassword);
            Assert.True(pass);
            Assert.Equal(Roles.Admin, role);
        }

        [Fact]
        public async void CreateAndAuthAccount()
        {
            string login = "test";
            string password = "1234";
            
            DBCreateResult result = await authService.TryCreateUserAsync(login, password);
            Assert.Equal(DBCreateResult.Success, result);

            var (pass, role) = await authService.TryLogInAsync(login, password);
            Assert.True(pass);
            Assert.Equal(Roles.User, role);
        }

        [Theory]
        [InlineData("1234", "C67C01D337283A1941D21384869CD322F9654746C5AAE6F84B7AE56F8B6E399F")]
        [InlineData("asdf 1234#@", "F000BA3D0B392F43DDA3F3183BF44F4B034BB7F00977FD10489C3F839A4D0B2A")]
        public void AuthHashTest(string password, string hash)
        {
            string hashCalced = AuthService.HashString(password);

            Assert.Equal(hash, hashCalced);
        }
    }
}