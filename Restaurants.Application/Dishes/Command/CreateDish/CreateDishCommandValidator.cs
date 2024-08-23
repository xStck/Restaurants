using FluentValidation;

namespace Restaurants.Application.Dishes.Command.CreateDish;

public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
{
    public CreateDishCommandValidator()
    {
        RuleFor(p => p.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0");
        RuleFor(p => p.KiloCalories).GreaterThanOrEqualTo(0).WithMessage("KiloCalories must be greater than or equal to 0");
    }
}