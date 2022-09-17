using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using vineyard_backend.Context;
using vineyard_backend.Services;
using vineyard_backend.Models;

var Port = int.Parse(Environment.GetEnvironmentVariable("VINEYARD_APP_PORT") ?? "3000");

string dbConnection = "Server=51.250.23.5;Port=9003;Database=vino;user id=postgres;password=root;";

var version = new VersionInfo
{
    Name = "Vineyard Backend Service",
    Version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "undefined",
    Start = DateTime.UtcNow,
};

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    serverOptions.Listen(IPAddress.Any, Port, listenOptions =>
    {
        listenOptions.UseConnectionLogging();
    });
});
builder.Services.AddDbContextPool<VineContext>(options => options.EnableSensitiveDataLogging().UseNpgsql(dbConnection));

builder.Services.AddSingleton(x => new VersionService(version));

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = version.Name,
            Version = version.VersionString,
            Description = $"Started: <b>{version.Start:u}</b>",
        });
        options.MapType<TimeSpan>(() => new OpenApiSchema { Type = "string", Format = "$date-span", Example = new OpenApiString(TimeSpan.Zero.ToString()) });
        Directory.GetFiles(AppContext.BaseDirectory, "*.xml").ToList().ForEach(xmlFilePath => options.IncludeXmlComments(xmlFilePath));
    }
);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
        });
});

var app = builder.Build();

app.UseCors();
app.UseHttpLogging();
app.UseSwagger();
app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = string.Empty;
        options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{version.Name} {version.VersionString}");
    }
);

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
