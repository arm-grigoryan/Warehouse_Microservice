using FluentValidation;
using Warehouse.API.Contracts.Orders;

namespace Warehouse.API.Validations.Orders;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.ProductId).GreaterThan(default(int));

        RuleFor(x => x.Quantity).GreaterThan(default(int));
    }
}
