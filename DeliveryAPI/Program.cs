using DeliveryAPI.Repository.Interfaces;
using DeliveryAPI.Repository.Repositories;
using DeliveryAPI.Service.Interfaces;
using DeliveryAPI.Service.Services;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// ===========================
// Dependency Injection
// ===========================
builder.Services.AddScoped<ILogisticsRepository, LogisticsRepository>();
builder.Services.AddScoped<ILogisticsService, LogisticsService>();

// ===========================
// Redis Configuration
// ===========================
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisConnection = builder.Configuration.GetConnectionString("Redis");

    var options = ConfigurationOptions.Parse(redisConnection!);
    options.AbortOnConnectFail = false;
    options.ConnectRetry = 3;
    options.ConnectTimeout = 5000;
    options.SyncTimeout = 5000;

    return ConnectionMultiplexer.Connect(options);
});

// ===========================
// CORS
// ===========================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "https://delivery-app-27d95.web.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// ===========================
// Swagger
// ===========================
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Delivery API",
        Version = "v1",
        Description = "Delivery Management API"
    });
});

var app = builder.Build();

// ===========================
// Swagger
// ===========================
app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Delivery API V1");
    options.RoutePrefix = "swagger";
});

// Uncomment if HTTPS is configured
// app.UseHttpsRedirection();

app.UseRouting();

// Enable CORS
app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

// Redirect root URL to Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();