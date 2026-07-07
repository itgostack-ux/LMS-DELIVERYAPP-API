using DeliveryAPI.Repository.Interfaces;
using DeliveryAPI.Repository.Repositories;
using DeliveryAPI.Service.Interfaces;
using DeliveryAPI.Service.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Dependency Injection
builder.Services.AddScoped<ILogisticsRepository, LogisticsRepository>();
builder.Services.AddScoped<ILogisticsService, LogisticsService>();

// CORS
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

// Swagger
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

// Enable Swagger in ALL environments
app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Delivery API V1");
    options.RoutePrefix = "swagger";
});

// Keep this commented until HTTPS is configured in IIS
// app.UseHttpsRedirection();

app.UseRouting();

// Enable CORS
app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

// Redirect root URL to Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();