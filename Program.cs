using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.RateLimiting;
using Data.Context;
using Services;
using Services.Interfaces;
using Microsoft.OpenApi.Models;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Loads environment variables from .env file
builder.Configuration.AddEnvironmentVariables();

// Loads Connection String from .env file
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? throw new InvalidOperationException("DATABASE_URL is not set");

builder.Services.AddDbContext<CantoApiContext>(options => options.UseNpgsql(connectionString));

// Add services to the container.
builder.Services.AddControllers()
    .ConfigureApplicationPartManager(manager =>
    {
    });

// Add this after AddControllers()
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;  // Makes URLs lowercase
    options.LowercaseQueryStrings = true;
});

// Configure Swagger with improved OpenAPI support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "CantoApi", 
        Version = "v1" 
    });
});

// Configure Swagger UI options separately
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// Add Rate Limiting (new in .NET 9.0)
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
});

// Register Services
builder.Services.AddScoped<IPhraseService, PhraseService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "canto-api/v1/swagger/{documentName}/swagger.json";
    });
    
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/canto-api/v1/swagger/v1/swagger.json", "CantoApi v1");
        c.RoutePrefix = "canto-api/v1/swagger";
    });
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => Results.Ok(new
{
    api_path = "/canto-api/v1"
}));

// Add this after your other route configurations
app.MapGet("canto-api/v1", () => Results.Ok(new
{
    apiName = "Canto API",
    version = "v1",
    status = "running",
    timestamp = DateTime.UtcNow,
    documentation = "/canto-api/v1/swagger",
}));

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Running in Development");
}

if (app.Environment.IsProduction())
{
    Console.WriteLine("Running in Production");
}
app.Run();