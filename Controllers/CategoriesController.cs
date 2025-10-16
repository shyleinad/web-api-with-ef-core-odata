using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using web_api_with_ef_core_odata.Models;
using web_api_with_ef_core_odata.Services;

namespace web_api_with_ef_core_odata.Controllers;

public class CategoriesController : ODataController
{
    private readonly ICategoryService service;

    public CategoriesController(ICategoryService service)
    {
        this.service = service;
    }

    [EnableQuery]
    public async Task<IActionResult> Get()
    {
        var categories = await service.GetAllAsync();

        return Ok(categories);
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] int key)
    {
        var category = await service.GetByIdAsync(key);

        return category == null ? NotFound() : Ok(category);
    }

    public async Task<IActionResult> Post([FromBody] Category category)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await service.CreateAsync(category);

        return Created(category);
    }

    public async Task<IActionResult> Put([FromRoute] int key, [FromBody] Category updateData)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var category = await service.UpdateAsync(key, updateData);

        return category == null ? NotFound() : Updated(updateData);
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
