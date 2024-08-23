using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IDishesRepository
{
    Task<int> CreateAsync(Dish dish);
    Task DeleteAllDishesAsync(IEnumerable<Dish> dishes);
    Task DeleteDishAsync(Dish dish);
}