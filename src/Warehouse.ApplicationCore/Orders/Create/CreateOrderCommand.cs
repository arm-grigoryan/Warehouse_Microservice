using MediatR;
using Warehouse.ApplicationCore.Orders.DTOs;

namespace Warehouse.ApplicationCore.Orders.Create;

public record CreateOrderCommand(int ProductId, int Quantity) : IRequest<OrderDto>;
