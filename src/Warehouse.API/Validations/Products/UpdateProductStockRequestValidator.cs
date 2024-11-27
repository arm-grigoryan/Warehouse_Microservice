using FluentValidation;
using Warehouse.API.Contracts.Products;

namespace Warehouse.API.Validations.Products;

public class UpdateProductStockRequestValidator : AbstractValidator<UpdateProductStockRequest>
{
    public UpdateProductStockRequestValidator()
    {
        RuleFor(x => x.Stock)
            .GreaterThan(default(int));
    }
}
