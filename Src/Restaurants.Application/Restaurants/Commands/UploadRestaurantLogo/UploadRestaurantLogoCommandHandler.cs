using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommandHandler(
    ILogger<UploadRestaurantLogoCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IRestaurantAuthorizationService authorizationService,
    IBlobStorageService blobStorageService) : IRequestHandler<UploadRestaurantLogoCommand>
{
    public async Task Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Upload restaurant logo to restaurant with id: {id}"   , request.RestaurantId);        
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if(restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        
        if(!authorizationService.Authorize(restaurant, ResourceOperation.Update))
            throw new ForbidException(); 
        
        var logoUrl = await blobStorageService.UploadToBlobAsync(request.File, request.FileName);
         restaurant.LogoUrl = logoUrl;

         await restaurantsRepository.SaveChangesAsync();
    }
}