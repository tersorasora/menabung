using Microsoft.EntityFrameworkCore;
using data;
using Repositories;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add DB Context with PostgreSQL
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add Services
builder.Services.AddScoped<IUserService, UserService>();

// Add Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Swagger only for dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();  

app.Run();
