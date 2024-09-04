using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Command.DeleteDishById;

public class DeleteDishByIdForRestaurantCommandHandler(
    ILogger<DeleteDishByIdForRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService)
    : IRequestHandler<DeleteDishByIdForRestaurantCommand>
{
    public async Task Handle(DeleteDishByIdForRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting dish with id {@DishId} for restaurant with id: {@RestaurantId}", request.DishId,
            request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
            throw new ForbidException();
        var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId);
        if (dish is null)
            throw new NotFoundException(nameof(Dish), request.DishId.ToString());
        await dishesRepository.DeleteDishAsync(dish);
    }
}