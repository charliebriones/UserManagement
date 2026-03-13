using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Services;
using UserManagement.Domain.Interfaces;
using UserManagement.Infrastructure.Data;
using UserManagement.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

#region Services

// Controllers
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Swagger (Development only)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Hardcoded CORS (GitHub Pages + Local Dev)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy
            .WithOrigins(
                "https://charliebriones.github.io",
                "http://localhost:4200",
                "http://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

#endregion

var app = builder.Build();

#region Middleware

// Swagger only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "User Management API V1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// ✅ CORS must come before MapControllers
app.UseCors("AllowAngularClient");

app.UseAuthorization();

// Map controllers (includes HealthController)
app.MapControllers();

#endregion

app.Run();