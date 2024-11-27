using FluentValidation;
using Warehouse.API.Contracts.Products;

namespace Warehouse.API.Validations.Products;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationLiterals.ProductNameMaxCharacters)
            .WithMessage(string.Format(ValidationLiterals.ProductNameExceedsCharactersMessage, ValidationLiterals.ProductNameMaxCharacters));

        RuleFor(x => x.CategoryId)
            .NotEmpty();

        RuleFor(x => x.Stock)
            .GreaterThan(default(int));

        RuleFor(x => x.LowStockThreshold)
            .GreaterThan(default(int));

        RuleFor(x => x.OutOfStockThreshold)
            .GreaterThanOrEqualTo(default(int));

        RuleFor(x => x.LowStockThreshold)
            .GreaterThan(x => x.OutOfStockThreshold)
            .WithMessage("Low stock threshold must be greater than out of stock threshold.");
    }
}
