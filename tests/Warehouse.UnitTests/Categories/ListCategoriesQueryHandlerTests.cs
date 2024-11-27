using AutoMapper;
using Warehouse.ApplicationCore.Categories.DTOs;
using Warehouse.ApplicationCore.Categories.List;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.UnitTests.Categories;

public class ListCategoriesQueryHandlerTests
{
    private readonly ICategoriesRepository _repository = Substitute.For<ICategoriesRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    [Fact]
    public async Task Handle_Should_ReturnMappedList()
    {
        // Arrange
        var query = new ListCategoriesQuery();
        var categories = new List<Category>
        {
            new Category { Id = "1", Name = "Category 1", Description = "Description 1" },
            new Category { Id = "2", Name = "Category 2", Description = "Description 2" }
        };
        var categoryDtos = new List<CategoryDto>
        {
            new CategoryDto ("1", "Category 1", "Description 1" ),
            new CategoryDto ("2", "Category 2", "Description 2")
        };

        _repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(categories));
        _mapper.Map<List<CategoryDto>>(categories).Returns(categoryDtos);

        var handler = new ListCategoriesQueryHandler(_repository, _mapper);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(categoryDtos);
        await _repository.Received(1).GetAllAsync(Arg.Any<CancellationToken>());
    }
}