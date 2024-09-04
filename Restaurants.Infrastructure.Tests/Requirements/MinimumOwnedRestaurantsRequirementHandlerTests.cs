using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.User;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Autorization.Requirements;

namespace Restaurants.Infrastructure.Tests.Requirements;

public class MinimumOwnedRestaurantsRequirementHandlerTests
{
    [Fact]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
    {
        //arrange
        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>
        {
            new()
            {
                OwnerId = currentUser.Id
            },
            new()
            {
                OwnerId = currentUser.Id
            },
            new()
            {
                OwnerId = "3"
            }
        };
        var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantRepositoryMock.Setup(r => r.GetByUserIdAsync(currentUser.Id))
            .ReturnsAsync(restaurants.Where(r => r.OwnerId == currentUser.Id));

        var loggerMock = new Mock<ILogger<MinimumOwnedRestaurantsRequirementHandler>>();

        var requirement = new MinimumOwnedRestaurantsRequirement(2);
        var requirementHandler = new MinimumOwnedRestaurantsRequirementHandler(loggerMock.Object,
            userContextMock.Object, restaurantRepositoryMock.Object);
        var context = new AuthorizationHandlerContext([requirement], null, null);

        //act
        await requirementHandler.HandleAsync(context);

        //assert
        context.HasSucceeded.Should().BeTrue();
    }
    
    [Fact]
    public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
    {
        //arrange
        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>
        {
            new()
            {
                OwnerId = currentUser.Id
            },
            new()
            {
                OwnerId = "2"
            },
            new()
            {
                OwnerId = "3"
            }
        };
        var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantRepositoryMock.Setup(r => r.GetByUserIdAsync(currentUser.Id))
            .ReturnsAsync(restaurants.Where(r => r.OwnerId == currentUser.Id));

        var loggerMock = new Mock<ILogger<MinimumOwnedRestaurantsRequirementHandler>>();

        var requirement = new MinimumOwnedRestaurantsRequirement(2);
        var requirementHandler = new MinimumOwnedRestaurantsRequirementHandler(loggerMock.Object,
            userContextMock.Object, restaurantRepositoryMock.Object);
        var context = new AuthorizationHandlerContext([requirement], null, null);

        //act
        await requirementHandler.HandleAsync(context);

        //assert
        context.HasFailed.Should().BeTrue();
    }
}