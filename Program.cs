using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net;
using System.Reflection;
using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using vineyard_backend.Converters;
using vineyard_backend.Context;
using vineyard_backend.Services;
using vineyard_backend.Models;
using vineyard_backend.Filters;

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

builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionFilter>();
            })
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals; 
        options.JsonSerializerOptions.Converters.Add(new DoubleConverter());
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }).ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errorMessage = string.Join(string.Empty, actionContext.ModelState.Values.SelectMany(item => item.Errors)
                        .Select(err => err.ErrorMessage + " " + err.Exception));

                    var errors = actionContext.ModelState.ToDictionary(error => error.Key, error => error.Value.Errors.Select(e => e.ErrorMessage).ToArray())
                        .Where(x => x.Value.Any());

                    var result = new BadRequestObjectResult(new
                    {
                        Code = 107,
                        Message = "Input validation error.",
                        Description = $"Sorry, it didn't work this time. {errorMessage}Please correct the error(s) and try again.",
                        errors,
                    });
                    result.ContentTypes.Add(MediaTypeNames.Application.Json);
                    return result;
                };
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
