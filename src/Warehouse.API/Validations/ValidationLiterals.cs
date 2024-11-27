namespace Warehouse.API.Validations;

public static class ValidationLiterals
{
    public const int ProductNameMaxCharacters = 100;
    public const string ProductNameExceedsCharactersMessage = "Product name cannot exceed {0} characters.";

    public const int CategoryNameMaxCharacters = 100;
    public const int CategoryDescriptionMaxCharacters = 512;
}
