using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
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
    public IActionResult Get()
    {
        logger.LogInformation("Fetching all categories...");

        var categories = context.Categories.ToList();

        logger.LogInformation("Fetched {Count} categories.", categories.Count);

        return Ok(categories);
    }

    [EnableQuery]
    public IActionResult Get([FromRoute] int key)
    {
        logger.LogInformation("Fetching category with ID: {Key}...", key);

        var category = context.Categories.FirstOrDefault(p => p.Id == key);

        if (category == null)
        {
            logger.LogWarning("Category with ID: {Key} not found.", key);

            return new NotFoundResult();
        }

        logger.LogInformation("Fetched category: {@Category}", category);

        return Ok(category);
    }

    public IActionResult Post([FromBody] Category category)
    {
        logger.LogInformation("Creating a new category: {@Category}...", category);

        context.Categories.Add(category);

        context.SaveChanges();

        logger.LogInformation("Category created successfully with ID {Key}.", category.Id);

        return Created(category);
    }

    public IActionResult Put([FromRoute] int key, [FromBody] Category updateData)
    {
        logger.LogInformation("Updating category with ID: {Key}...", key);

        var category = context.Categories.FirstOrDefault(p => p.Id == key);

        if (category == null)
        {
            logger.LogWarning("Category with ID: {Key} not found.", key);

            return new NotFoundResult();
        }

        category.Name = updateData.Name;

        context.SaveChanges();

        logger.LogInformation("Category with ID: {Key} updated successfully.", key);

        return Updated(updateData);
    }

    public IActionResult Delete([FromRoute] int key)
    {
        logger.LogInformation("Deleting category with ID: {Key}...", key);

        var category = context.Categories.FirstOrDefault(p => p.Id == key);

        if (category == null)
        {
            logger.LogWarning("Category with ID: {Key} not found.", key);

            return new NotFoundResult();
        }

        context.Categories.Remove(category);

        context.SaveChanges();

        logger.LogInformation("Category with ID: {Key} deleted successfully.", key);

        return NoContent();
    }

}
