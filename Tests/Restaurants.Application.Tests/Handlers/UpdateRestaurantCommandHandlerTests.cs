using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Tests.Handlers;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly UpdateRestaurantCommandHandler _handler;
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;
    private readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock;

    public UpdateRestaurantCommandHandlerTests()
    {
        _restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _mapperMock = new Mock<IMapper>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
        _handler = new UpdateRestaurantCommandHandler(_loggerMock.Object, _restaurantRepositoryMock.Object,
            _mapperMock.Object, _restaurantAuthorizationServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ForValidCommand_UpdateRestaurant()
    {
        //arrange
        var restaurantId = 1;
        var restaurant = new Restaurant { Id = restaurantId };
        var updateRestaurantCommand = new UpdateRestaurantCommand { Id = restaurantId };

        _restaurantRepositoryMock.Setup(r =>
                r.GetByIdAsync(restaurantId))
            .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(s =>
                s.Authorize(It.IsAny<Restaurant>(), It.IsAny<ResourceOperation>()))
            .Returns(true);

        //arrange
        await _handler.Handle(updateRestaurantCommand, CancellationToken.None);

        //assert
        _restaurantAuthorizationServiceMock.Verify(s => s.Authorize(restaurant, ResourceOperation.Update),
            Times.Once);
        _mapperMock.Verify(m => m.Map(updateRestaurantCommand, restaurant), Times.Once);
        _restaurantRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ForNullRestaurant_ThrowsNotFoundException()
    {
        //arrange
        var restaurantId = 2;
        var updateRestaurantCommand = new UpdateRestaurantCommand { Id = restaurantId };

        _restaurantRepositoryMock.Setup(r =>
                r.GetByIdAsync(restaurantId))
            .ReturnsAsync((Restaurant?)null);

        //arrange
        var func = async () => await _handler.Handle(updateRestaurantCommand, CancellationToken.None);

        //assert
        await func.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Restaurant with identifier \'{restaurantId}\' doesn't exist.");
    }

    [Fact]
    public async Task Handle_ForUnauthorizedUser_ThrowsForbidException()
    {
        //arrange
        var restaurantId = 3;
        var updateRestaurantCommand = new UpdateRestaurantCommand { Id = restaurantId };
        var restaurant = new Restaurant { Id = restaurantId };

        _restaurantRepositoryMock.Setup(r =>
                r.GetByIdAsync(restaurantId))
            .ReturnsAsync(restaurant);
        _restaurantAuthorizationServiceMock.Setup(s => s.Authorize(restaurant, ResourceOperation.Update))
            .Returns(false);

        //arrange
        var func = async () => await _handler.Handle(updateRestaurantCommand, CancellationToken.None);

        //assert
        await func.Should().ThrowAsync<ForbidException>();
    }
}