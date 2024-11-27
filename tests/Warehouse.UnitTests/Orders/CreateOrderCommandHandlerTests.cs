namespace Warehouse.UnitTests.Orders;

using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.ApplicationCore.Orders.Create;
using Warehouse.ApplicationCore.Orders.DTOs;
using Warehouse.ApplicationCore.Services.AvailableStockOrders;
using Warehouse.ApplicationCore.Services.LowStockOrders;
using Warehouse.ApplicationCore.Services.PendingOrders;
using Warehouse.Domain.Exceptions;
using Warehouse.Domain.Interfaces.Persistence;
using Xunit;

public class CreateOrderCommandHandlerTests
{
    private readonly IProductsRepository _productsRepository = Substitute.For<IProductsRepository>();
    private readonly IAvailableStockOrdersService _availableStockOrdersService = Substitute.For<IAvailableStockOrdersService>();
    private readonly ILowStockOrdersService _lowStockOrdersService = Substitute.For<ILowStockOrdersService>();
    private readonly IPendingOrdersService _pendingOrdersService = Substitute.For<IPendingOrdersService>();
    private readonly IConfiguration _configuration = Substitute.For<IConfiguration>();
    private readonly IConfigurationSection _configurationSection = Substitute.For<IConfigurationSection>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    [Fact]
    public async Task Handle_Should_Call_ReserveDirectly_When_StockStatus_Is_Available()
    {
        // Arrange
        var command = new CreateOrderCommand(1, 5);
        var product = new Product { Id = command.ProductId, CategoryId = "1", Name = "Product 1", Stock = 10, LowStockThreshold = 4, OutOfStockThreshold = 0 };
        var order = new Order { Id = Guid.NewGuid().ToString(), ProductId = command.ProductId, Quantity = command.Quantity, Status = OrderStatus.Completed };
        var orderDto = new OrderDto { Id = Guid.NewGuid().ToString(), ProductId = command.ProductId, Quantity = command.Quantity, Status = OrderStatus.Completed };

        _productsRepository.GetByIdAsync(command.ProductId, Arg.Any<CancellationToken>())
                           .Returns(product);
        _availableStockOrdersService.ReserveDirectlyAsync(product, command.Quantity, Arg.Any<CancellationToken>())
                                    .Returns(order);

        _configurationSection.Value.Returns("true");
        _configuration.GetSection("RejectOutOfStockRequests").Returns(_configurationSection);

        _mapper.Map<OrderDto>(order).Returns(orderDto);

        var handler = new CreateOrderCommandHandler(
            _productsRepository, _availableStockOrdersService, _lowStockOrdersService,
            _pendingOrdersService, _configuration, _mapper);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(orderDto);
        await _availableStockOrdersService.Received(1)
            .ReserveDirectlyAsync(product, command.Quantity, Arg.Any<CancellationToken>());
        await _lowStockOrdersService.DidNotReceiveWithAnyArgs().ReserveWithManualApprovalAsync(default, default, default);
        await _pendingOrdersService.DidNotReceiveWithAnyArgs().CreateOrderAsync(default, default, default);
    }

    [Fact]
    public async Task Handle_Should_Call_ReserveWithManualApproval_When_StockStatus_Is_LowStock()
    {
        // Arrange
        var command = new CreateOrderCommand(1, 5);
        var product = new Product { Id = command.ProductId, CategoryId = "1", Name = "Product 1", Stock = 3, LowStockThreshold = 4, OutOfStockThreshold = 0 };
        var order = new Order { Id = Guid.NewGuid().ToString(), ProductId = command.ProductId, Quantity = command.Quantity, Status = OrderStatus.UnderReview };
        var orderDto = new OrderDto { Id = Guid.NewGuid().ToString(), ProductId = command.ProductId, Quantity = command.Quantity, Status = OrderStatus.UnderReview };

        _productsRepository.GetByIdAsync(command.ProductId, Arg.Any<CancellationToken>())
                           .Returns(product);
        _lowStockOrdersService.ReserveWithManualApprovalAsync(product, command.Quantity, Arg.Any<CancellationToken>())
                              .Returns(order);

        _configurationSection.Value.Returns("true");
        _configuration.GetSection("RejectOutOfStockRequests").Returns(_configurationSection);

        _mapper.Map<OrderDto>(order).Returns(orderDto);

        var handler = new CreateOrderCommandHandler(
            _productsRepository, _availableStockOrdersService, _lowStockOrdersService,
            _pendingOrdersService, _configuration, _mapper);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(orderDto);
        await _lowStockOrdersService.Received(1)
            .ReserveWithManualApprovalAsync(product, command.Quantity, Arg.Any<CancellationToken>());
        await _availableStockOrdersService.DidNotReceiveWithAnyArgs().ReserveDirectlyAsync(default, default, default);
        await _pendingOrdersService.DidNotReceiveWithAnyArgs().CreateOrderAsync(default, default, default);
    }

    [Fact]
    public async Task Handle_Should_Call_CreateOrderAsync_When_StockStatus_Is_OutOfStock_And_Rejection_Disabled()
    {
        // Arrange
        var command = new CreateOrderCommand(1, 5);
        var product = new Product { Id = command.ProductId, CategoryId = "1", Name = "Product 1", Stock = 0, LowStockThreshold = 4, OutOfStockThreshold = 1 };
        var order = new PendingOrder { Id = Guid.NewGuid().ToString(), ProductId = command.ProductId, Quantity = command.Quantity, Status = OrderStatus.InsufficientStock };
        var orderDto = new OrderDto { Id = Guid.NewGuid().ToString(), ProductId = command.ProductId, Quantity = command.Quantity, Status = OrderStatus.InsufficientStock };

        _productsRepository.GetByIdAsync(command.ProductId, Arg.Any<CancellationToken>())
                           .Returns(product);
        _pendingOrdersService.CreateOrderAsync(product.Id, command.Quantity, Arg.Any<CancellationToken>())
                             .Returns(Task.FromResult(order));

        _configurationSection.Value.Returns("false");
        _configuration.GetSection("RejectOutOfStockRequests").Returns(_configurationSection);

        _mapper.Map<OrderDto>(order).Returns(orderDto);

        var handler = new CreateOrderCommandHandler(
            _productsRepository, _availableStockOrdersService, _lowStockOrdersService,
            _pendingOrdersService, _configuration, _mapper);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(orderDto);
        await _pendingOrdersService.Received(1)
            .CreateOrderAsync(product.Id, command.Quantity, Arg.Any<CancellationToken>());
        await _availableStockOrdersService.DidNotReceiveWithAnyArgs().ReserveDirectlyAsync(default, default, default);
        await _lowStockOrdersService.DidNotReceiveWithAnyArgs().ReserveWithManualApprovalAsync(default, default, default);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_StockStatus_Is_OutOfStock_And_Rejection_Enabled()
    {
        // Arrange
        var command = new CreateOrderCommand(1, 5);
        var product = new Product { Id = command.ProductId, CategoryId = "1", Name = "Product 1", Stock = 0, LowStockThreshold = 4, OutOfStockThreshold = 1 };

        _productsRepository.GetByIdAsync(command.ProductId, Arg.Any<CancellationToken>())
                           .Returns(product);
        _configurationSection.Value.Returns("true");
        _configuration.GetSection("RejectOutOfStockRequests").Returns(_configurationSection);

        var handler = new CreateOrderCommandHandler(
            _productsRepository, _availableStockOrdersService, _lowStockOrdersService,
            _pendingOrdersService, _configuration, _mapper);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<WarehouseException>()
            .WithMessage("The selected product is currently out of stock.")
            .Where(e => e.StatusCode == (int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Product_Not_Found()
    {
        // Arrange
        var command = new CreateOrderCommand(1, 5);
        _productsRepository.GetByIdAsync(command.ProductId, Arg.Any<CancellationToken>())
                           .Returns((Product)null);

        var handler = new CreateOrderCommandHandler(
            _productsRepository, _availableStockOrdersService, _lowStockOrdersService,
            _pendingOrdersService, _configuration, _mapper);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<WarehouseException>()
            .WithMessage($"Product with ID : {command.ProductId} not found.")
            .Where(e => e.StatusCode == (int)HttpStatusCode.NotFound);
    }
}