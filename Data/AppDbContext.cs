using Microsoft.EntityFrameworkCore;
using ProductEfConsole.Models;

namespace ProductEfConsole.Data;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Environment.GetEnvironmentVariable("PRODUCT_EF_CONNECTION_STRING")
            ?? "Host=localhost;Port=5432;Database=product_ef_console;Username=postgres;Password=your_password";

        optionsBuilder.UseNpgsql(connectionString);
    }
}
