using MediatR;

namespace Restaurants.Application.Dishes.Command.DeleteDishById;

public class DeleteDishByIdForRestaurantCommand(int restaurantId, int dishId) : IRequest
{
    public int RestaurantId { get; } = restaurantId;
    public int DishId { get; } = dishId;
}