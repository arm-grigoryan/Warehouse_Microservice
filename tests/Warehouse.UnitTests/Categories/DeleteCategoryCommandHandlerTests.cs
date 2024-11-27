using Warehouse.ApplicationCore.Categories.Delete;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.UnitTests.Categories;

public class DeleteCategoryCommandHandlerTests
{
    private readonly ICategoriesRepository _repository = Substitute.For<ICategoriesRepository>();

    [Fact]
    public async Task Handle_Should_DeleteCategory_And_ReturnTrue()
    {
        // Arrange
        var command = new DeleteCategoryCommand("123");
        _repository.DeleteAsync(command.Id, Arg.Any<CancellationToken>()).Returns(Task.FromResult(true));

        var handler = new DeleteCategoryCommandHandler(_repository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        await _repository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
    }
}
