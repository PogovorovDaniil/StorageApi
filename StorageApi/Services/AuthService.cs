using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StorageApi.Models;
using StorageApi.Models.Contexts;
using StorageApi.Models.DBO.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StorageApi.Services
{
    public class AuthService
    {
        private readonly AuthConfiguration _authConfiguration;
        private readonly AuthorizationContext _context;

        private const string rootLogin = "root";

        public AuthService(AuthConfiguration authConfiguration, AuthorizationContext context)
        {
            _authConfiguration = authConfiguration;
            _context = context;
        }

        public async Task<(bool pass, string role)> TryLogInAsync(string login, string password)
        {
            if (login == rootLogin && password == _authConfiguration.RootPassword)
            {
                return (true, Roles.Admin);
            }

            if (await _context.Users.AnyAsync(u => u.Login == login && u.PasswordHash == HashString(password)))
            {
                return (true, Roles.User);
            }

            return (false, "");
        }

        public async Task<DBCreateResult> TryCreateUserAsync(string login, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Login == login))
            {
                return DBCreateResult.AlreadyExist;
            }
            await _context.Users.AddAsync(new User()
            {
                Login = login,
                PasswordHash = HashString(password)
            });
            if (await _context.SaveChangesAsync() == 1)
            {
                return DBCreateResult.Success;
            }
            return DBCreateResult.UnknownError;
        }

        public string GetNewToken(string login, string role)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, login), new Claim(ClaimTypes.Role, role) };
            var jwt = new JwtSecurityToken(
                issuer: _authConfiguration.Issuer,
                audience: _authConfiguration.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(_authConfiguration.IssuerSigningKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private static string HashString(string text)
        {
            const string salt = "88df38cb6d304c5ead276cad5662455e";
            using (SHA256 hash = SHA256.Create())
            {
                byte[] passCode = Encoding.UTF8.GetBytes(salt + text);
                byte[] hashCode = hash.ComputeHash(passCode);
                return string.Join("", hashCode.Select(c => c.ToString("X2")));
            }
        }
    }
}
