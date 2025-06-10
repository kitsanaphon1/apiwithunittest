using Microsoft.EntityFrameworkCore;
using WebApp.API.Models;

namespace WebApp.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users => Set<User>();
}
