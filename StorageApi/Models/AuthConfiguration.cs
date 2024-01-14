using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace StorageApi.Models
{
    public class AuthConfiguration
    {
        private readonly IConfiguration _configuration;
        public AuthConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Issuer => _configuration["Auth:Issuer"];
        public string Audience => _configuration["Auth:Audience"];
        private SymmetricSecurityKey _issuerSigningKey;
        public SymmetricSecurityKey IssuerSigningKey => _issuerSigningKey ?? (_issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Auth:IssuerSigningKey"])));

        public string RootPassword => _configuration["Auth:RootPassword"];
    }
}
