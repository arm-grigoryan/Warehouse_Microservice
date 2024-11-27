using AutoMapper;
using MediatR;
using Warehouse.ApplicationCore.Orders.DTOs;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.ApplicationCore.Orders.List;

public class GetOrdersHistoryQueryHandler(
    IOrdersRepository ordersRepository,
    IMapper mapper) : IRequestHandler<GetOrdersHistoryQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetOrdersHistoryQuery request, CancellationToken cancellationToken)
    {
        var oders = await ordersRepository.GetAllAsync(cancellationToken);
        return mapper.Map<List<OrderDto>>(oders);
    }
}
