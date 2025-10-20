using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using data;
using Repositories;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add DB Context with PostgreSQL
// builder.Services.AddDbContext<AppDBContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreServer_Default")));

// Add DB Context for Production with PostgreSQL
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreServer_Production")));

// Add DB Context with SQL Server
// builder.Services.AddDbContext<AppDBContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer_Default")));

// Add DB Context for Production with SQL Server
// builder.Services.AddDbContext<AppDBContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer_Production")));

// accept case sensitive JSON
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
});

// Add CORS policy and allow frontend access
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173", // React Vite default port
            "http://localhost:5008"  // Blazor Server default port
            ) 
              .WithExposedHeaders("Authorization")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// Add Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITransactionServices, TransactionServices>();

// Add Controllers
builder.Services.AddControllers();

// Add Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key not found in configuration"));

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(jwtKey)
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("AllowFrontend");
// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
