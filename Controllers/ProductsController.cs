using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using web_api_with_ef_core_odata.Data;
using web_api_with_ef_core_odata.Models;

namespace web_api_with_ef_core_odata.Controllers;

public class ProductsController : ODataController
{
    private readonly ProjectDbContext context;
    private readonly ILogger<ProductsController> logger;

    public ProductsController(ProjectDbContext context, ILogger<ProductsController> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    [EnableQuery]
    public async Task<IActionResult> Get()
    {
        logger.LogInformation("Fetching all products...");

        var products = await context.Products.Include(p => p.Category).ToListAsync();

        logger.LogInformation("Fetched {Count} products.", products.Count);

        return Ok(products);
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] int key)
    {
        logger.LogInformation("Fetching product with ID: {Key}...", key);

        var product = await context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == key);

        if (product == null)
        {
            logger.LogWarning("Product with ID: {Key} not found.", key);

            return NotFound();
        }

        logger.LogInformation("Fetching product: {@Product}", product);

        return Ok(product);
    }

    public async Task<IActionResult> Post([FromBody] Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        logger.LogInformation("Creating a new product: {@Product}...", product);

        context.Products.Add(product);

        await context.SaveChangesAsync();

        logger.LogInformation("Product created successfully with ID {Key}.", product.Id);

        return Created(product);
    }

    public async Task<IActionResult> Put([FromRoute] int key, [FromBody] Product updateData) 
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        logger.LogInformation("Updating product with ID: {Key}...", key);

        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == key);

        if (product == null)
        {
            logger.LogWarning("Product with ID: {Key} not found.", key);

            return NotFound();
        }

        product.Name = updateData.Name;
        product.CategoryId = updateData.CategoryId;
        product.Price = updateData.Price;

        await context.SaveChangesAsync();

        logger.LogInformation("Product with ID: {Key} updated successfully.", key);

        return Updated(updateData);
    }

    public async Task<IActionResult> Delete([FromRoute] int key)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        logger.LogInformation("Deleting product with ID: {Key}...", key);

        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == key);

        if (product == null)
        {
            logger.LogWarning("Product with ID: {Key} not found.", key);

            return NotFound();
        }

        context.Products.Remove(product);

        await context.SaveChangesAsync();

        logger.LogInformation("Product with ID: {Key} deleted successfully.", key);

        return NoContent();
    }
}
