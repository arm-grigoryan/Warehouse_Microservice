using AutoMapper;
using Warehouse.ApplicationCore.Categories.DTOs;
using Warehouse.Domain.Entities;

namespace Warehouse.ApplicationCore.Categories.Mapping;

public class CategoriesMappingProfile : Profile
{
    public CategoriesMappingProfile()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
    }
}
