using FluentValidation;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private int[] allowPageSizes = [5, 10, 15, 30];

    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThanOrEqualTo(1);
        
        RuleFor(r => r.PageSize)
            .Must(allowPageSizes.Contains)
            .WithMessage($"Page size must be in [{string.Join(",", allowPageSizes)}]");
    }
}