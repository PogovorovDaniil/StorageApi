using Microsoft.IdentityModel.Tokens;
using StorageApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StorageApi.Helper
{
    public static class AuthHelper
    {
        public static string GetNewToken(AuthConfiguration authConfiguration, string login, string role = Roles.User)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, login), new Claim(ClaimTypes.Role, role) };
            var jwt = new JwtSecurityToken(
                issuer: authConfiguration.Issuer,
                audience: authConfiguration.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(authConfiguration.IssuerSigningKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static string HashString(string text)
        {
            using (SHA256 hash = SHA256.Create())
            {
                byte[] passCode = Encoding.UTF8.GetBytes(text);
                byte[] hashCode = hash.ComputeHash(passCode);
                return string.Join("", hashCode.Select(c => c.ToString("X2")));
            }
        }
    }
}
