using AutoMapper;
using Warehouse.ApplicationCore.Products.DTOs;
using Warehouse.Domain.Entities;

namespace Warehouse.ApplicationCore.Products.Mapping;

public class ProductsMappingProfile : Profile
{
    public ProductsMappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}
