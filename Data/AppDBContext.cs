using Microsoft.EntityFrameworkCore;
using Models;

namespace data;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

    public DbSet<User> users { get; set; }
    public DbSet<Transaction> transactions { get; set; }
    public DbSet<Roles> roles { get; set; }
}