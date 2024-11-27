using MediatR;
using Warehouse.ApplicationCore.Services.PendingOrders;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.ApplicationCore.Products.UpdateStock;

public class UpdateProductStockCommandHandler(
    IProductsRepository repository,
    IPendingOrdersService pendingOrdersService) : IRequestHandler<UpdateProductStockCommand>
{
    public async Task Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
    {
        await repository.UpdateStockAsync(request.ProductId, request.Stock, cancellationToken);

        // FulFill pending orders
        await pendingOrdersService.FulfillOrdersAsync(request.ProductId, cancellationToken);
    }
}
