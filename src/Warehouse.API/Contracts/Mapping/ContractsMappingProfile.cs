using AutoMapper;
using Warehouse.API.Contracts.Categories;
using Warehouse.API.Contracts.Orders;
using Warehouse.API.Contracts.Products;
using Warehouse.ApplicationCore.Categories.Create;
using Warehouse.ApplicationCore.Categories.Update;
using Warehouse.ApplicationCore.Orders.Create;
using Warehouse.ApplicationCore.Products.Create;
using Warehouse.ApplicationCore.Products.Update;

namespace Warehouse.API.Contracts.Mapping;

public class ContractsMappingProfile : Profile
{
    public ContractsMappingProfile()
    {
        CreateMap<CreateOrderRequest, CreateOrderCommand>();

        CreateMap<CreateProductRequest, CreateProductCommand>();
        CreateMap<UpdateProductRequest, UpdateProductCommand>();

        CreateMap<CreateCategoryRequest, CreateCategoryCommand>();
        CreateMap<UpdateCategoryRequest, UpdateCategoryCommand>();
    }
}
