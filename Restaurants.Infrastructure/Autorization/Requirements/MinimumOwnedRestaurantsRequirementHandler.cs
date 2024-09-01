using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.User;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Autorization.Requirements;

public class MinimumOwnedRestaurantsRequirementHandler(ILogger<MinimumOwnedRestaurantsRequirementHandler> logger,
    IUserContext userContext,
    IRestaurantsRepository restaurantsRepository) : AuthorizationHandler<MinimumOwnedRestaurantsRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumOwnedRestaurantsRequirement requirement)
    {
        var user = userContext.GetCurrentUser();
        var restaurants = await restaurantsRepository.GetByUserIdAsync(user.Id);
        if (restaurants.Count() < requirement.MinimumOwnedRestaurants)
        {
            logger.LogInformation("Minimum owned restaurants authorization failed.");
            context.Fail();
        }
        logger.LogInformation("Minimum owned restaurants authorization succeeded.");
        context.Succeed(requirement);
    }
}