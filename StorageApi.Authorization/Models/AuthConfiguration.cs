using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace StorageApi.Authorization.Models
{
    public class AuthConfiguration
    {
        private readonly IConfiguration configuration;
        public AuthConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Issuer => configuration["Auth:Issuer"];
        public string Audience => configuration["Auth:Audience"];
        private SymmetricSecurityKey issuerSigningKey;
        public SymmetricSecurityKey IssuerSigningKey => issuerSigningKey ?? (issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Auth:IssuerSigningKey"])));

        public string RootPassword => configuration["Auth:RootPassword"];
    }
}
