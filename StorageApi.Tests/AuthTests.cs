using StorageApi.Authorization.Services;

namespace StorageApi.Tests
{
    public class AuthTests
    {
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