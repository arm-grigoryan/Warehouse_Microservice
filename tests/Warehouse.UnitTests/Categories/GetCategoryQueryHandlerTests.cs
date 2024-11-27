using AutoMapper;
using Warehouse.ApplicationCore.Categories.DTOs;
using Warehouse.ApplicationCore.Categories.Get;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.UnitTests.Categories;

public class GetCategoryQueryHandlerTests
{
    private readonly ICategoriesRepository _repository = Substitute.For<ICategoriesRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    [Fact]
    public async Task Handle_Should_ReturnMappedDto()
    {
        // Arrange
        var query = new GetCategoryQuery("123");
        var category = new Category { Id = "123", Name = "Test Category", Description = "Test Description" };
        var categoryDto = new CategoryDto("123", category.Name, category.Description);

        _repository.GetByIdAsync(query.CategoryId, Arg.Any<CancellationToken>())
                   .Returns(Task.FromResult(category));
        _mapper.Map<CategoryDto>(category).Returns(categoryDto);

        var handler = new GetCategoryQueryHandler(_repository, _mapper);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(categoryDto);
        await _repository.Received(1).GetByIdAsync(query.CategoryId, Arg.Any<CancellationToken>());
    }
}