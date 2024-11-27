using Warehouse.ApplicationCore.Services.LowStockOrders;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Enums;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.ApplicationCore.Services.PendingOrders;

internal class PendingOrdersService(
    IUnitOfWork unitOfWork,
    ILowStockOrdersService lowStockOrdersService,
    IPendingOrdersRepository pendingOrdersRepository,
    IOrdersRepository ordersRepository,
    IProductsRepository productsRepository) : IPendingOrdersService
{
    public Task<PendingOrder> CreateOrderAsync(int productId, int quantity, CancellationToken cancellationToken = default)
    {
        // Create and save the pending order
        var pendingOrder = new PendingOrder
        {
            ProductId = productId,
            Quantity = quantity,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.InsufficientStock
        };

        return pendingOrdersRepository.AddAsync(pendingOrder, cancellationToken);
    }

    public async Task FulfillOrdersAsync(int productId, CancellationToken cToken = default)
    {
        var product = await productsRepository.GetByIdAsync(productId, cToken);
        var pendingOrders = await pendingOrdersRepository.GetAllByProductIdAsync(productId, cToken);

        foreach (var order in pendingOrders.OrderBy(o => o.OrderDate))
        {
            if (product.Stock < order.Quantity)
            {
                return;
            }

            if (product.Status == StockStatus.Available)
            {
                // Begin transaction
                //await unitOfWork.BeginTransactionAsync(cToken);

                try
                {
                    // Should pass session to repositories

                    // Deduct stock
                    product.Stock -= order.Quantity;
                    await productsRepository.UpdateStockAsync(productId, product.Stock, cToken);

                    // Add completed order to history
                    var completedOrder = new Order
                    {
                        ProductId = productId,
                        Quantity = order.Quantity,
                        Status = OrderStatus.Completed,
                        OrderDate = order.OrderDate,
                    };
                    await ordersRepository.AddAsync(completedOrder, cToken);

                    // Remove pending order from collection
                    await pendingOrdersRepository.DeleteAsync(order.Id, cToken);

                    // Commit transaction
                    //await unitOfWork.CommitTransactionAsync(cToken);
                }
                catch
                {
                    // Abort transaction
                    //await unitOfWork.AbortTransactionAsync(cToken);
                    throw;
                }
            }
            else if (product.Status == StockStatus.LowStock)
            {
                await lowStockOrdersService.ReserveWithManualApprovalAsync(product, order.Quantity, cToken);
            }
        }
    }
}
