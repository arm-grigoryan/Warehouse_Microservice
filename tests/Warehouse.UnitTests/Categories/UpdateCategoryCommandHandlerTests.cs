using AutoMapper;
using Warehouse.ApplicationCore.Categories.DTOs;
using Warehouse.ApplicationCore.Categories.Update;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.UnitTests.Categories;

public class UpdateCategoryCommandHandlerTests
{
    private readonly ICategoriesRepository _repository = Substitute.For<ICategoriesRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    [Fact]
    public async Task Handle_Should_UpdateCategory_And_ReturnMappedDto()
    {
        // Arrange
        var command = new UpdateCategoryCommand("123", "Updated Name", "Updated Description");
        var category = new Category { Id = command.Id, Name = command.Name, Description = command.Description };
        var categoryDto = new CategoryDto(command.Id, command.Name, command.Description);

        _repository.UpdateAsync(Arg.Any<Category>(), Arg.Any<CancellationToken>())
                   .Returns(Task.FromResult(category));
        _mapper.Map<CategoryDto>(category).Returns(categoryDto);

        var handler = new UpdateCategoryCommandHandler(_repository, _mapper);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(categoryDto);
        await _repository.Received(1).UpdateAsync(Arg.Is<Category>(c =>
            c.Id == command.Id && c.Name == command.Name && c.Description == command.Description), Arg.Any<CancellationToken>());
    }
}