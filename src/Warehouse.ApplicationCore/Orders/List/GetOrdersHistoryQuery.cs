using MediatR;
using Warehouse.ApplicationCore.Orders.DTOs;

namespace Warehouse.ApplicationCore.Orders.List;

public record GetOrdersHistoryQuery : IRequest<List<OrderDto>>;
