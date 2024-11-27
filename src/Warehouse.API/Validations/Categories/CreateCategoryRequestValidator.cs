using FluentValidation;
using Warehouse.API.Contracts.Categories;

namespace Warehouse.API.Validations.Categories;

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationLiterals.CategoryNameMaxCharacters);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(ValidationLiterals.CategoryDescriptionMaxCharacters);
    }
}
