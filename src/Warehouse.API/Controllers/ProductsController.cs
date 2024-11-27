using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Warehouse.API.Contracts.Products;
using Warehouse.ApplicationCore.Products.Create;
using Warehouse.ApplicationCore.Products.DTOs;
using Warehouse.ApplicationCore.Products.Get;
using Warehouse.ApplicationCore.Products.Update;
using Warehouse.ApplicationCore.Products.UpdateStock;

namespace Warehouse.API.Controllers;

[Route(ApiLiterals.Route)]
[ApiController]
public class ProductsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductRequest request)
    {
        var command = mapper.Map<CreateProductCommand>(request);
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetProduct), new { id = result.Id }, result);
    }

    [HttpPut]
    public async Task<ActionResult<ProductDto>> UpdateProduct([FromBody] UpdateProductRequest request)
    {
        var command = mapper.Map<UpdateProductCommand>(request);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id:int}/stock")]
    public async Task<IActionResult> UpdateProductStock(int id, [FromBody] UpdateProductStockRequest request)
    {
        var command = new UpdateProductStockCommand(id, request.Stock);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var result = await mediator.Send(new GetProductQuery(id));

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
