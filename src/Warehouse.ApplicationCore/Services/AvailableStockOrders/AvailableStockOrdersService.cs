using System.Net;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Enums;
using Warehouse.Domain.Exceptions;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.ApplicationCore.Services.AvailableStockOrders;

internal class AvailableStockOrdersService(
    IUnitOfWork unitOfWork,
    IProductsRepository productsRepository,
    IOrdersRepository ordersRepository) : IAvailableStockOrdersService
{
    public async Task<Order> ReserveDirectlyAsync(Product product, int quantity, CancellationToken cToken = default)
    {
        var newStockAmount = product.Stock - quantity;
        if (newStockAmount < 0)
        {
            throw new WarehouseException("Insufficient stock to fulfill the order.", HttpStatusCode.BadRequest);
        }

        // Begin transaction
        //await unitOfWork.BeginTransactionAsync(cToken);

        try
        {
            // Should pass session to repositories

            // Update product stock
            await productsRepository.UpdateStockAsync(product.Id, newStockAmount, cToken);

            // Create and save the order
            var order = new Order
            {
                ProductId = product.Id,
                Quantity = quantity,
                Status = OrderStatus.Completed,
                OrderDate = DateTime.UtcNow
            };

            var createdOrder = await ordersRepository.AddAsync(order, cToken);

            // Commit transaction
            //await unitOfWork.CommitTransactionAsync(cToken);

            return createdOrder;
        }
        catch
        {
            // Abort transaction
            //await unitOfWork.AbortTransactionAsync(cToken);
            throw;
        }
    }
}
