using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.RateLimiting;
using Data.Context;
using Services;
using Services.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .ConfigureApplicationPartManager(manager =>
    {
        // Optional: Configure controller discovery
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

// Add DbContext
builder.Services.AddDbContext<CantoApiContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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