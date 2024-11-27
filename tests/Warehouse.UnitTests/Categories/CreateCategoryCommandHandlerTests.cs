using AutoMapper;
using Warehouse.ApplicationCore.Categories.Create;
using Warehouse.ApplicationCore.Categories.DTOs;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.UnitTests.Categories;

public class CreateCategoryCommandHandlerTests
{
    private readonly ICategoriesRepository _repository = Substitute.For<ICategoriesRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    [Fact]
    public async Task Handle_Should_AddCategory_And_ReturnMappedDto()
    {
        // Arrange
        var command = new CreateCategoryCommand("Test Category", "Test Description");
        var category = new Category { Name = command.Name, Description = command.Description };
        var categoryDto = new CategoryDto (Guid.NewGuid().ToString()[..7], command.Name, command.Description);

        _repository.AddAsync(Arg.Any<Category>(), Arg.Any<CancellationToken>())
                   .Returns(Task.FromResult(category));
        _mapper.Map<CategoryDto>(category).Returns(categoryDto);

        var handler = new CreateCategoryCommandHandler(_repository, _mapper);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(categoryDto);
        await _repository.Received(1).AddAsync(Arg.Is<Category>(c =>
            c.Name == command.Name && c.Description == command.Description), Arg.Any<CancellationToken>());
    }
}