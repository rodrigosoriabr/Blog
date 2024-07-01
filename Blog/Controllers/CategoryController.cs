using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAsync(
        [FromServices] BlogDataContext context)
    {
        try
        {
            var categories = await context.Categories.ToListAsync();

            return Ok(new ResultViewModel<List<Category>>(categories));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("CATGETEX500 - Internal server error"));
        }
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetAsync(
        [FromServices] BlogDataContext context,
        [FromRoute] int id)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null) return NotFound(new ResultViewModel<Category>("Category not found"));

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Category>("CATGETBYEX500 - Internal server error"));
        }
    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync(
        [FromServices] BlogDataContext context,
        [FromBody] EditorCategoryViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

        try
        {
            var category = new Category
            {
                Name = model.Name,
                Slug = model.Slug
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Category>("CATPOSTDB500 - Fail when creating a new category"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Category>("CATPOSTEX500 - Internal server error"));
        }
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync(
        [FromServices] BlogDataContext context,
        [FromRoute] int id,
        [FromBody] EditorCategoryViewModel model)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null) return NotFound(new ResultViewModel<Category>("Category not found"));

            category.Name = model.Name;
            category.Slug = model.Slug;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Category>("CATPUTDB500 - Fail when updating a new category"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Category>("CATPUTEX500 - Internal server error"));
        }
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
        [FromServices] BlogDataContext context,
        [FromRoute] int id)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null) return NotFound(new ResultViewModel<Category>("Category not found"));

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Category>("CATDELDB500 - Fail when deleting a category"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Category>("CATDELEX500 - Internal server error"));
        }
    }
}