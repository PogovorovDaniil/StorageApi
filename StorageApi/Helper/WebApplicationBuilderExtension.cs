using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StorageApi.Models.Context;
using System.Text;

namespace StorageApi.Helper
{
    public static class WebApplicationBuilderExtension
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

        public static void ConfigureAuthorization(this WebApplicationBuilder builder) => builder.Services
            .AddAuthorization()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Auth:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Auth:Audience"],

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Auth:IssuerSigningKey"])),
                };
            });

        public static void ConfigureSwagger(this WebApplicationBuilder builder) => builder.Services.AddSwaggerGen(setupAction =>
            {
                setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
    }
}
