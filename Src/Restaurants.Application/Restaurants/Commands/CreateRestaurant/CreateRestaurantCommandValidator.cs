using FluentValidation;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;

namespace Restaurants.Application.Restaurants.Validators;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> validCategories = ["Italian", "Mexican", "Japanese", "American", "Polish", "Indian"];

    public CreateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 100);
        RuleFor(dto => dto.Category)
            // .Must(category => validCategories.Contains(category))
            .Must(validCategories.Contains)
            .WithMessage("Invalid category. Please choose from the valid categories.");
        // .Custom((value, context) =>
        // {
        //     var isValidCategory = validCategories.Contains(value);
        //     if (!isValidCategory)
        //     {
        //         context.AddFailure("Category", "Invalid category. Please choose from the valid categories.");
        //     }
        // });
        RuleFor(dto => dto.ContactEmail)
            .EmailAddress()
            .WithMessage("Please provide a valid email address");

        RuleFor(dto => dto.PostalCode)
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("Please provide a valid postal code (XX-XXX)");
    }
}