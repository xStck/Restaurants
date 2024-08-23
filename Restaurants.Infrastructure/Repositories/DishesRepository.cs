using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class DishesRepository(RestaurantsDbContext dbContext) : IDishesRepository
{
    public async Task<int> CreateAsync(Dish dish)
    {
        dbContext.Dishes.Add(dish);
        await dbContext.SaveChangesAsync();
        return dish.Id;
    }

    
    public async Task DeleteAllDishesAsync(IEnumerable<Dish> dishes)
    {
        dbContext.Dishes.RemoveRange(dishes);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteDishAsync(Dish dish)
    {
        dbContext.Dishes.Remove(dish);
        await dbContext.SaveChangesAsync();
    }
}