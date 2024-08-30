using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using todolist_api.Models;
using todolist_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Load .env file and configure fallback logic
Env.Load();
ConfigureDatabase(builder);

// Add services to the container.

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowLocalhost",
            builder =>
            {
                builder.WithOrigins("http://localhost:3100")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
    });

// Configure caching and other services
builder.Services.AddMemoryCache();
builder.Services.AddScoped<TodoService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// cors
app.UseCors("AllowLocalhost");

// Configure middleware / http pipeline for development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Common middleware configuration for all environments
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.Run();

// Configure database services based on environment variables or fallback
void ConfigureDatabase(WebApplicationBuilder builder)
{
    // Attempt to get the connection string from environment variables
    var connectionString = Environment.GetEnvironmentVariable("ConnectionString");

    if (string.IsNullOrEmpty(connectionString))
    {
        // No connection string found, fallback to InMemory database
        Console.WriteLine("Connection string not found. Using InMemory fallback database.");
        builder.Services.AddDbContext<TodoDbContext>(options =>
            options.UseInMemoryDatabase("FallbackDb"));
    }
    else
    {
        // Connection string found, use SQL Server
        Console.WriteLine("Using SQL Server database.");
        builder.Services.AddDbContext<TodoDbContext>(options =>
            options.UseSqlServer(connectionString));
    }
}