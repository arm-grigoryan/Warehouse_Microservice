using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Warehouse.API.Contracts.Categories;
using Warehouse.ApplicationCore.Categories.Create;
using Warehouse.ApplicationCore.Categories.Delete;
using Warehouse.ApplicationCore.Categories.DTOs;
using Warehouse.ApplicationCore.Categories.Get;
using Warehouse.ApplicationCore.Categories.List;
using Warehouse.ApplicationCore.Categories.Update;

namespace Warehouse.API.Controllers;

[Route(ApiLiterals.Route)]
[ApiController]
public class CategoriesController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryRequest request)
    {
        var command = mapper.Map<CreateCategoryCommand>(request);
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetCategory), new { id = result.Id }, result);
    }

    [HttpPut]
    public async Task<ActionResult<CategoryDto>> UpdateCategory([FromBody] UpdateCategoryRequest request)
    {
        var command = mapper.Map<UpdateCategoryCommand>(request);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteCategory(string id)
    {
        await mediator.Send(new DeleteCategoryCommand(id));
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(string id)
    {
        var result = await mediator.Send(new GetCategoryQuery(id));

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories()
    {
        var result = await mediator.Send(new ListCategoriesQuery());
        return Ok(result);
    }
}
