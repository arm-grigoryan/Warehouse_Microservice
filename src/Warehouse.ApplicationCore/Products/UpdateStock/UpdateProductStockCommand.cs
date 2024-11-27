using MediatR;

namespace Warehouse.ApplicationCore.Products.UpdateStock;

public record UpdateProductStockCommand(int ProductId, int Stock) : IRequest;
