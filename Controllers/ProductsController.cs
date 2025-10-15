using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using web_api_with_ef_core_odata.Data;
using web_api_with_ef_core_odata.Models;

namespace web_api_with_ef_core_odata.Controllers;

public class ProductsController : ODataController
{
    private readonly ProductContext context;

    public ProductsController(ProductContext context)
    {
        this.context = context;
    }

    [EnableQuery]
    public IActionResult Get()
    {
        var products = context.Products.ToList();

        return Ok(products);
    }

    [EnableQuery]
    public IActionResult Get([FromRoute] int key)
    {
        var product = context.Products.FirstOrDefault(p => p.Id == key);

        if (product == null)
        {
            return new NotFoundResult();
        }

        return Ok(product);
    }

    public IActionResult Post([FromBody] Product product)
    {
        context.Products.Add(product);

        context.SaveChanges();

        return Created(product);
    }

    public IActionResult Put([FromRoute] int key, [FromBody] Product updateData) 
    {
        var product = context.Products.FirstOrDefault(p => p.Id == updateData.Id);

        if (product == null)
        {
            return new NotFoundResult();
        }

        product.Name = updateData.Name;
        product.CategoryId = updateData.CategoryId;
        product.Price = updateData.Price;

        context.SaveChanges();

        return Updated(updateData);
    }

    public IActionResult Delete([FromRoute] int key)
    {
        var product = context.Products.FirstOrDefault(p => p.Id == key);

        if (product == null)
        {
            return new NotFoundResult();
        }

        context.Products.Remove(product);

        context.SaveChanges();

        return NoContent();
    }
}
