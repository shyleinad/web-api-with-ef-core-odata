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
    public IActionResult Get()
    {
        logger.LogInformation("Fetching all products...");

        var products = context.Products.Include(p => p.Category).ToList();

        logger.LogInformation("Fetched {Count} products.", products.Count);

        return Ok(products);
    }

    [EnableQuery]
    public IActionResult Get([FromRoute] int key)
    {
        logger.LogInformation("Fetching product with ID: {Key}...", key);

        var product = context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == key);

        if (product == null)
        {
            logger.LogWarning("Product with ID: {Key} not found.", key);

            return new NotFoundResult();
        }

        logger.LogInformation("Fetching product: {@Product}", product);

        return Ok(product);
    }

    public IActionResult Post([FromBody] Product product)
    {
        logger.LogInformation("Creating a new product: {@Product}...", product);

        context.Products.Add(product);

        context.SaveChanges();

        logger.LogInformation("Product created successfully with ID {Key}.", product.Id);

        return Created(product);
    }

    public IActionResult Put([FromRoute] int key, [FromBody] Product updateData) 
    {
        logger.LogInformation("Updating product with ID: {Key}...", key);

        var product = context.Products.FirstOrDefault(p => p.Id == key);

        if (product == null)
        {
            logger.LogWarning("Product with ID: {Key} not found.", key);

            return new NotFoundResult();
        }

        product.Name = updateData.Name;
        product.CategoryId = updateData.CategoryId;
        product.Price = updateData.Price;

        context.SaveChanges();

        logger.LogInformation("Product with ID: {Key} updated successfully.", key);

        return Updated(updateData);
    }

    public IActionResult Delete([FromRoute] int key)
    {
        logger.LogInformation("Deleting product with ID: {Key}...", key);

        var product = context.Products.FirstOrDefault(p => p.Id == key);

        if (product == null)
        {
            logger.LogWarning("Product with ID: {Key} not found.", key);

            return new NotFoundResult();
        }

        context.Products.Remove(product);

        context.SaveChanges();

        logger.LogInformation("Product with ID: {Key} deleted successfully.", key);

        return NoContent();
    }
}
