using AutoMapper;
using Warehouse.Domain.Entities;
using Warehouse.Persistence.Data.Entities;

namespace Warehouse.Persistence.Data.Mappings;

internal class DbEntitiesMappings : Profile
{
    public DbEntitiesMappings()
    {
        CreateMap<Category, CategoryDb>().ReverseMap();

        CreateMap<Order, OrderDb>().ReverseMap();

        CreateMap<Product, ProductDb>().ReverseMap();

        CreateMap<PendingOrder, PendingOrderDb>().ReverseMap();
    }
}
