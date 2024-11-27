using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Warehouse.ApplicationCore.Categories.Create;
using Warehouse.ApplicationCore.Categories.Mapping;
using Warehouse.ApplicationCore.Products.Create;
using Warehouse.ApplicationCore.Products.Mapping;
using Warehouse.Persistence.Data;
using Warehouse.Persistence.Data.Entities;
using Warehouse.Persistence.Data.Mappings;
using Warehouse.Persistence.Data.Repositories;
using Warehouse.Persistence.Settings;

namespace Warehouse.IntegrationTests.Data.Products;

public class CreateProductIntegrationTests : IClassFixture<TestDatabaseFixture>
{
    private readonly WarehouseDbContext _dbContext;
    private readonly IOptions<WarehouseDBSettings> _settings;

    public CreateProductIntegrationTests(TestDatabaseFixture fixture)
    {
        _dbContext = fixture.WarehouseDbContext;
        _settings = fixture.Settings;
    }

    [Fact]
    public async Task Handle_Should_SaveProduct_ToDatabase()
    {
        // Arrange
        var mapper = new MapperConfiguration(cfg => cfg.AddProfiles([new DbEntitiesMappings(), new ProductsMappingProfile()])).CreateMapper();
        var counterService = new CounterService(_settings, _dbContext);
        var repository = new ProductsRepository(counterService, _settings, _dbContext, mapper);
        var handler = new CreateProductCommandHandler(repository, mapper);

        var productName = "Test Product";
        var command = new CreateProductCommand(productName, "1", 15, 4, 0);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        var collection = _dbContext.Database.GetCollection<ProductDb>(_settings.Value.ProductsCollectionName);
        var savedProduct = await collection.Find(c => c.Id == result.Id).FirstOrDefaultAsync();

        result.Should().NotBeNull();
        savedProduct.Should().NotBeNull();
        savedProduct.Name.Should().Be(productName);
    }
}