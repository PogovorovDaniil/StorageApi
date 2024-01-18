using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using StorageApi.Authorization;
using StorageApi.Core.Helpers;
using StorageApi.Database;
using StorageApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<StorageService>(); // TODO
builder.AddAuthServices();
builder.AddDbContexts();
builder.ConfigureSwagger();
builder.ConfigureAuthorization();

builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
