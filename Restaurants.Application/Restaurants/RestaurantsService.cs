using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(
    IRestaurantsRepository restaurantsRepository,
    ILogger<RestaurantsService> logger,
    IMapper mapper)
    : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDto>> GetAllAsync()
    {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await restaurantsRepository.GetAllAsync();
        return mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
    }

    public async Task<RestaurantDto?> GetByIdAsync(int id)
    {
        logger.LogInformation("Getting restaurant by id");
        var restaurant = await restaurantsRepository.GetByIdAsync(id);
        return mapper.Map<RestaurantDto?>(restaurant);
    }

    public async Task<int> CreateAsync(CreateRestaurantDto createRestaurantDto)
    {
        logger.LogInformation("Creating new restaurant");
        var restaurant = mapper.Map<Restaurant>(createRestaurantDto);
        var id = await restaurantsRepository.CreateAsync(restaurant);
        return id;
    }
}