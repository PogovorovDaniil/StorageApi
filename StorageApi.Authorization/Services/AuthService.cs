﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StorageApi.Authorization.Models;
using StorageApi.Core.Models.Constants;
using StorageApi.Database.Contexts;
using StorageApi.Database.Models.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StorageApi.Authorization.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthConfiguration authConfiguration;
        private readonly AuthorizationContext context;

        private const string rootLogin = "root";

        public AuthService(AuthConfiguration authConfiguration, AuthorizationContext context)
        {
            this.authConfiguration = authConfiguration;
            this.context = context;
        }

        public async Task<(bool pass, string role)> TryLogInAsync(string login, string password)
        {
            if (login == rootLogin && password == authConfiguration.RootPassword)
            {
                return (true, Roles.Admin);
            }

            if (await context.Users.AnyAsync(u => u.Login == login && u.PasswordHash == HashString(password)))
            {
                return (true, Roles.User);
            }

            return (false, "");
        }

        public async Task<DBCreateResult> TryCreateUserAsync(string login, string password)
        {
            if (await context.Users.AnyAsync(u => u.Login == login))
            {
                return DBCreateResult.AlreadyExist;
            }
            await context.Users.AddAsync(new User()
            {
                Login = login,
                PasswordHash = HashString(password)
            });
            if (await context.SaveChangesAsync() == 1)
            {
                return DBCreateResult.Success;
            }
            return DBCreateResult.UnknownError;
        }

        public string GetNewToken(string login, string role)
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
