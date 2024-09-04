using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Autorization.Requirements;

public class MinimumOwnedRestaurantsRequirement(int minimumOwnedRestaurants) : IAuthorizationRequirement
{
    public int MinimumOwnedRestaurants { get; } = minimumOwnedRestaurants;
}