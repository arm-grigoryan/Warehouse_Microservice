using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Warehouse.API.Contracts.Orders;
using Warehouse.ApplicationCore.Orders.Create;
using Warehouse.ApplicationCore.Orders.DTOs;
using Warehouse.ApplicationCore.Orders.List;

namespace Warehouse.API.Controllers;

[Route(ApiLiterals.Route)]
[ApiController]
public class OrdersController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<OrderDto>> PlaceOrder([FromBody] CreateOrderRequest request)
    {
        var command = mapper.Map<CreateOrderCommand>(request);
        var result = await mediator.Send(command);
        return CreatedAtAction(null, result);
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetOrderHistory()
    {
        var result = await mediator.Send(new GetOrdersHistoryQuery());
        return Ok(result);
    }
}
