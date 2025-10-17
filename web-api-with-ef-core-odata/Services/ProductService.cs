using Microsoft.EntityFrameworkCore;
using web_api_with_ef_core_odata.Data;
using web_api_with_ef_core_odata.Models;

namespace web_api_with_ef_core_odata.Services;

public class ProductService : IProductService
{
    private readonly ProjectDbContext context;
    private readonly ILogger<ProductService> logger;

    public ProductService(ProjectDbContext context, ILogger<ProductService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        logger.LogInformation("Creating a new product: {@Product}...", product);

        context.Products.Add(product);

        await context.SaveChangesAsync();

        logger.LogInformation("Product created successfully with ID {Key}.", product.Id);

        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        logger.LogInformation("Deleting product with ID: {Key}...", id);

        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            logger.LogWarning("Product with ID: {Key} not found.", id);

            return false;
        }

        context.Products.Remove(product);

        await context.SaveChangesAsync();

        logger.LogInformation("Product with ID: {Key} deleted successfully.", id);

        return true;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        logger.LogInformation("Fetching all products...");

        var products = await context.Products.Include(p => p.Category).ToListAsync();

        logger.LogInformation("Fetched {Count} products.", products.Count);

        return products;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        logger.LogInformation("Fetching product with ID: {Key}...", id);

        var product = await context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

        logger.LogInformation("Fetched product: {@Product}", product);

        return product;
    }

    public async Task<Product?> UpdateAsync(int id, Product updateData)
    {
        logger.LogInformation("Updating product with ID: {Key}...", id);

        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            logger.LogWarning("Product with ID: {Key} not found.", id);

            return null;
        }

        product.Name = updateData.Name;
        product.CategoryId = updateData.CategoryId;
        product.Price = updateData.Price;

        await context.SaveChangesAsync();

        logger.LogInformation("Product updated: {@Product}", product);

        return product;
    }
}
