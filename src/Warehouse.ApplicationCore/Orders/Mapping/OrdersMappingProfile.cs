using AutoMapper;
using Warehouse.ApplicationCore.Orders.DTOs;
using Warehouse.Domain.Entities;

namespace Warehouse.ApplicationCore.Orders.Mapping;

public class OrdersMappingProfile : Profile
{
    public OrdersMappingProfile()
    {
        CreateMap<Order, OrderDto>().ReverseMap();
    }
}
