using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Autorization.Requirements;

public class MinimumAgeRequirement(int minimumAge) : IAuthorizationRequirement
{
    public int MinimumAge { get; } = minimumAge;
}