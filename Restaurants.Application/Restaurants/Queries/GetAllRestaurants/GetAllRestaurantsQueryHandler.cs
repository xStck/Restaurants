using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler(
    IRestaurantsRepository restaurantsRepository,
    ILogger<GetAllRestaurantsQueryHandler> logger,
    IMapper mapper) : IRequestHandler<GetAllRestaurantsQuery, PageResult<RestaurantDto>>
{
    public async Task<PageResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all restaurants");
        var (restaurants, totalCount) = await restaurantsRepository.GetAllMatchingAsync(request.SearchPhrase, request.PageSize, request.PageNumber);
        var restaurantsDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        var pageResult = new PageResult<RestaurantDto>(restaurantsDtos, totalCount, request.PageSize, request.PageNumber);
        return pageResult;
    }
}