using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<Restaurant?> GetByIdAsync(int id);
    Task<IEnumerable<Restaurant>> GetByUserIdAsync(string userId);
    Task<int> CreateAsync(Restaurant restaurant);
    Task DeleteAsync(Restaurant restaurant);
    Task SaveChangesAsync();
    Task<IEnumerable<Restaurant>> GetAllMatchingAsync(string? searchPhrase);
}