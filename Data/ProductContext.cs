using Microsoft.EntityFrameworkCore;
using web_api_with_ef_core_odata.Models;

namespace web_api_with_ef_core_odata.Data;

public class ProductContext : DbContext
{
    public DbSet<Models.Product> Products { get; set; }

    public ProductContext(DbContextOptions<ProductContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop", CategoryId = 1, Price = 1200 },
            new Product { Id = 2, Name = "Keyboard", CategoryId = 1, Price = 100 },
            new Product { Id = 3, Name = "Chair", CategoryId = 2, Price = 200 },
            new Product { Id = 4, Name = "Desk Lamp", CategoryId = 3, Price = 60 }
        );
    }
}
