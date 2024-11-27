using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Warehouse.ApplicationCore.Categories.Create;
using Warehouse.ApplicationCore.Categories.Mapping;
using Warehouse.Persistence.Data;
using Warehouse.Persistence.Data.Entities;
using Warehouse.Persistence.Data.Mappings;
using Warehouse.Persistence.Data.Repositories;
using Warehouse.Persistence.Settings;

namespace Warehouse.IntegrationTests.Data.Categories;

public class CreateCategoryIntegrationTests : IClassFixture<TestDatabaseFixture>
{
    private readonly WarehouseDbContext _dbContext;
    private readonly IOptions<WarehouseDBSettings> _settings;

    public CreateCategoryIntegrationTests(TestDatabaseFixture fixture)
    {
        _dbContext = fixture.WarehouseDbContext;
        _settings = fixture.Settings;
    }

    [Fact]
    public async Task Handle_Should_SaveCategory_ToDatabase()
    {
        // Arrange
        var mapper = new MapperConfiguration(cfg => cfg.AddProfiles([new DbEntitiesMappings(), new CategoriesMappingProfile()])).CreateMapper();
        var repository = new CategoriesRepository(_settings, _dbContext, mapper);
        var handler = new CreateCategoryCommandHandler(repository, mapper);

        var categoryName = "Test Category";
        var categoryDescription = "Test Description";
        var command = new CreateCategoryCommand(categoryName, categoryDescription);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        var collection = _dbContext.Database.GetCollection<CategoryDb>(_settings.Value.CategoriesCollectionName);
        var savedCategory = await collection.Find(c => c.Id == result.Id).FirstOrDefaultAsync();

        result.Should().NotBeNull();
        savedCategory.Should().NotBeNull();
        savedCategory.Name.Should().Be(categoryName);
        savedCategory.Description.Should().Be(categoryDescription);
    }
}