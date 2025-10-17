using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using web_api_with_ef_core_odata.Models;
using web_api_with_ef_core_odata.Services;

namespace web_api_with_ef_core_odata.Controllers;

public class ProductsController : ODataController
{
    private readonly IProductService service;

    public ProductsController(IProductService service)
    {
        this.service = service;
    }

    [EnableQuery]
    public async Task<IActionResult> Get()
    {
        var products = await service.GetAllAsync();

        return Ok(products);
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] int key)
    {
        var product = await service.GetByIdAsync(key);

        return product == null ? NotFound() : Ok(product);
    }

    public async Task<IActionResult> Post([FromBody] Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await service.CreateAsync(product);

        return Created(product);
    }

    public async Task<IActionResult> Put([FromRoute] int key, [FromBody] Product updateData) 
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = await service.UpdateAsync(key, updateData);

        return product == null ? NotFound() : Updated(product);
    }

    public async Task<IActionResult> Delete([FromRoute] int key)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var deleted = await service.DeleteAsync(key);

        return deleted ? NoContent() : NotFound();
    }
}
