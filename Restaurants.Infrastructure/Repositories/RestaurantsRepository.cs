using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
{
    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var restaurants = await dbContext.Restaurants.Include(r => r.Dishes).ToListAsync();
        return restaurants;
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        var restaurant = await dbContext.Restaurants.Include(r => r.Dishes).FirstOrDefaultAsync(x => x.Id == id);
        return restaurant;
    }

    public async Task<IEnumerable<Restaurant>> GetByUserIdAsync(string userId)
    {
        var restaurants = await dbContext.Restaurants.Where(x => x.OwnerId == userId).ToListAsync();
        return restaurants;
    }

    public async Task<int> CreateAsync(Restaurant restaurant)
    {
        dbContext.Restaurants.Add(restaurant);
        await dbContext.SaveChangesAsync();
        return restaurant.Id;
    }

    public async Task DeleteAsync(Restaurant restaurant)
    {
        dbContext.Restaurants.Remove(restaurant);
        await dbContext.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Restaurant>> GetAllMatchingAsync(string? searchPhrase)
    {
        var searchPhraseLower = searchPhrase?.ToLower();
        var restaurants = await dbContext.Restaurants
            .Where(x => searchPhraseLower == null || 
                        x.Name.ToLower().Contains(searchPhraseLower) || 
                        x.Description.ToLower().Contains(searchPhraseLower))
            .Include(r => r.Dishes).ToListAsync(); 
        return restaurants;    }
}