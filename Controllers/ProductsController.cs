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
    [HttpGet]
    public IActionResult GetProducts()
    {
        var products = context.Products.ToList();

        return Ok(products);
    }

    [EnableQuery]
    [HttpGet("{key}")]
    public IActionResult GetProduct(int id)
    {
        var product = context.Products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return new NotFoundResult();
        }

        return Ok(product);
    }

    [HttpPost]
    public IActionResult CreateProduct([FromBody] Product product)
    {
        context.Products.Add(product);

        context.SaveChanges();

        return Created(product);
    }

    [HttpPut("{key}")]
    public IActionResult UpdateProduct([FromBody] Product updateData) 
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

    [HttpDelete("{key}")]
    public IActionResult DeleteProduct(int id)
    {
        var product = context.Products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return new NotFoundResult();
        }

        context.Products.Remove(product);

        context.SaveChanges();

        return NoContent();
    }
}
