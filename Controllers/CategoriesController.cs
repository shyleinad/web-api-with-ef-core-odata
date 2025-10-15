using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using web_api_with_ef_core_odata.Data;
using web_api_with_ef_core_odata.Models;

namespace web_api_with_ef_core_odata.Controllers;

public class CategoriesController : ODataController
{
    private readonly ProjectDbContext context;

    public CategoriesController(ProjectDbContext context)
    {
        this.context = context;
    }

    [EnableQuery]
    public IActionResult Get()
    {
        var categories = context.Categories.ToList();

        return Ok(categories);
    }

    [EnableQuery]
    public IActionResult Get([FromRoute] int key)
    {
        var categories = context.Categories.FirstOrDefault(p => p.Id == key);

        if (categories == null)
        {
            return new NotFoundResult();
        }

        return Ok(categories);
    }

    public IActionResult Post([FromBody] Category category)
    {
        context.Categories.Add(category);

        context.SaveChanges();

        return Created(category);
    }

    public IActionResult Put([FromRoute] int key, [FromBody] Category updateData)
    {
        var category = context.Categories.FirstOrDefault(p => p.Id == key);

        if (category == null)
        {
            return new NotFoundResult();
        }

        category.Name = updateData.Name;

        context.SaveChanges();

        return Updated(updateData);
    }

    public IActionResult Delete([FromRoute] int key)
    {
        var category = context.Categories.FirstOrDefault(p => p.Id == key);

        if (category == null)
        {
            return new NotFoundResult();
        }

        context.Categories.Remove(category);

        context.SaveChanges();

        return NoContent();
    }

}
