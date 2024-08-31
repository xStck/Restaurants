using System.Security.AccessControl;
using Microsoft.Extensions.Logging;
using Restaurants.Application.User;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Autorization.Services;

public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger,
    IUserContext userContext) : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperaion resourceType)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation("Authorizing user {UserEmail}, to {Operation} for restaurant {RestaurantName}",
            user.Email, resourceType, restaurant.Name);
        if (resourceType == ResourceOperaion.Read || resourceType == ResourceOperaion.Create)
        {
            logger.LogInformation("Create/Read operation - successful authorization");
            return true;
        }

        if (resourceType == ResourceOperaion.Delete && user.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("Admin user, delete operation - successful authorization");
            return true;
        }

        if ((resourceType == ResourceOperaion.Delete || resourceType == ResourceOperaion.Update) &&
            user.Id == restaurant.OwnerId)
        {
            logger.LogInformation("Restaurant owner - successful authorization");
            return true;
        }

        return false;

    }
}