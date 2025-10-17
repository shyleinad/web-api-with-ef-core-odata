using Microsoft.EntityFrameworkCore;
using web_api_with_ef_core_odata.Data;
using web_api_with_ef_core_odata.Models;

namespace web_api_with_ef_core_odata.Services;

public class CategoryService : ICategoryService
{
    private readonly ProjectDbContext context;
    private readonly ILogger<CategoryService> logger;

    public CategoryService(ProjectDbContext context, ILogger<CategoryService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<Category> CreateAsync(Category category)
    {
        logger.LogInformation("Creating a new category: {@Category}...", category);

        context.Categories.Add(category);

        await context.SaveChangesAsync();

        logger.LogInformation("Category created successfully with ID {Key}.", category.Id);

        return category;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        logger.LogInformation("Deleting category with ID: {Key}...", id);

        var category = await context.Categories.FirstOrDefaultAsync(p => p.Id == id);

        if (category == null)
        {
            logger.LogWarning("Category with ID: {Key} not found.", id);

            return false;
        }

        context.Categories.Remove(category);

        await context.SaveChangesAsync();

        logger.LogInformation("Category with ID: {Key} deleted successfully.", id);

        return true;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        logger.LogInformation("Fetching all categories...");

        var categories = await context.Categories.Include(p => p.Products).ToListAsync();

        logger.LogInformation("Fetched {Count} categories.", categories.Count);

        return categories;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        logger.LogInformation("Fetching category with ID: {Key}...", id);

        var category = await context.Categories.Include(p => p.Products).FirstOrDefaultAsync(p => p.Id == id);

        logger.LogInformation("Fetched category: {@Category}", category);

        return category;
    }

    public async Task<Category?> UpdateAsync(int id, Category updateData)
    {
        logger.LogInformation("Updating category with ID: {Key}...", id);

        var category = await context.Categories.FirstOrDefaultAsync(p => p.Id == id);

        if (category == null)
        {
            logger.LogWarning("Category with ID: {Key} not found.", id);

            return null;
        }

        category.Name = updateData.Name;

        await context.SaveChangesAsync();

        logger.LogInformation("Category updated: {@UpdatedCategory}", category);

        return category;
    }
}
