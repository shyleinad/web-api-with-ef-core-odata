using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using web_api_with_ef_core_odata.Data;
using web_api_with_ef_core_odata.Models;

namespace web_api_with_ef_core_odata.Controllers;

public class CategoriesController : ODataController
{
    private readonly ProjectDbContext context;
    private readonly ILogger<CategoriesController> logger;

    public CategoriesController(ProjectDbContext context, ILogger<CategoriesController> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    [EnableQuery]
    public async Task<IActionResult> Get()
    {
        logger.LogInformation("Fetching all categories...");

        var categories = await context.Categories.ToListAsync();

        logger.LogInformation("Fetched {Count} categories.", categories.Count);

        return Ok(categories);
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] int key)
    {
        logger.LogInformation("Fetching category with ID: {Key}...", key);

        var category = await context.Categories.FirstOrDefaultAsync(p => p.Id == key);

        if (category == null)
        {
            logger.LogWarning("Category with ID: {Key} not found.", key);

            return NotFound();
        }

        logger.LogInformation("Fetched category: {@Category}", category);

        return Ok(category);
    }

    public async Task<IActionResult> Post([FromBody] Category category)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        logger.LogInformation("Creating a new category: {@Category}...", category);

        context.Categories.Add(category);

        await context.SaveChangesAsync();

        logger.LogInformation("Category created successfully with ID {Key}.", category.Id);

        return Created(category);
    }

    public async Task<IActionResult> Put([FromRoute] int key, [FromBody] Category updateData)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        logger.LogInformation("Updating category with ID: {Key}...", key);

        var category = await context.Categories.FirstOrDefaultAsync(p => p.Id == key);

        if (category == null)
        {
            logger.LogWarning("Category with ID: {Key} not found.", key);

            return NotFound();
        }

        category.Name = updateData.Name;

        await context.SaveChangesAsync();

        logger.LogInformation("Category with ID: {Key} updated successfully.", key);

        return Updated(updateData);
    }

    public async Task<IActionResult> Delete([FromRoute] int key)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        logger.LogInformation("Deleting category with ID: {Key}...", key);

        var category = await context.Categories.FirstOrDefaultAsync(p => p.Id == key);

        if (category == null)
        {
            logger.LogWarning("Category with ID: {Key} not found.", key);

            return NotFound();
        }

        context.Categories.Remove(category);

        await context.SaveChangesAsync();

        logger.LogInformation("Category with ID: {Key} deleted successfully.", key);

        return NoContent();
    }

}
