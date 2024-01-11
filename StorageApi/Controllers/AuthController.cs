using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StorageApi.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StorageApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private AuthConfiguration _authConfiguration;
        public AuthController(AuthConfiguration authConfiguration) 
        {
            _authConfiguration = authConfiguration;
        }

        [HttpGet]
        [ProducesResponseType(typeof(AuthResult), 200)]
        public IActionResult Token(string login, string password)
        {
            if (login == "root" && password == _authConfiguration.RootPassword)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, login) };
                var jwt = new JwtSecurityToken(_authConfiguration.Issuer, _authConfiguration.Audience, claims,
                    signingCredentials: new SigningCredentials(_authConfiguration.IssuerSigningKey, SecurityAlgorithms.Aes128CbcHmacSha256));

                string jwtToken = new JwtSecurityTokenHandler().WriteToken(jwt);

                return new JsonResult(new AuthResult { Token = jwtToken });
            }

            return new UnauthorizedResult();
        }
    }
}
