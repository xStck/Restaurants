using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Tests.Mappers;

public class RestaurantProfileTests
{
    private readonly IMapper _mapper;

    public RestaurantProfileTests()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
            cfg.AddProfile<RestaurantsProfile>()
        );
        _mapper = mapperConfiguration.CreateMapper();
    }

    [Fact]
    public void Mapper_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        //arrange
        var restaurant = new Restaurant
        {
            Id = 1,
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Test Category",
            HasDelivery = true,
            ContactEmail = "test@test.com",
            ContactNumber = "123456789",
            Address = new Address
            {
                Street = "Test Street",
                City = "Test City",
                PostalCode = "11-111"
            }
        };

        //act
        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        //assert
        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(1);
        restaurantDto.Name.Should().Be("Test Restaurant");
        restaurantDto.Description.Should().Be("Test Description");
        restaurantDto.Category.Should().Be("Test Category");
        restaurantDto.HasDelivery.Should().BeTrue();
        restaurantDto.Street.Should().Be("Test Street");
        restaurantDto.City.Should().Be("Test City");
        restaurantDto.PostalCode.Should().Be("11-111");
    }

    [Fact]
    public void Mapper_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        //arrange
        var command = new CreateRestaurantCommand
        {
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Test Category",
            HasDelivery = true,
            ContactEmail = "test@test.com",
            ContactNumber = "123456789",
            City = "Test City",
            Street = "Test Street",
            PostalCode = "11-111"
        };

        //act
        var restaurant = _mapper.Map<Restaurant>(command);

        //assert
        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be("Test Restaurant");
        restaurant.Description.Should().Be("Test Description");
        restaurant.Category.Should().Be("Test Category");
        restaurant.HasDelivery.Should().BeTrue();
        restaurant.ContactEmail.Should().Be("test@test.com");
        restaurant.ContactNumber.Should().Be("123456789");
        restaurant.Address.Should().NotBeNull();
        restaurant.Address.Street.Should().Be("Test Street");
        restaurant.Address.City.Should().Be("Test City");
        restaurant.Address.PostalCode.Should().Be("11-111");
    }

    [Fact]
    public void Mapper_ForUpdateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        //arrange
        var command = new UpdateRestaurantCommand
        {
            Id = 1,
            Name = "Test Restaurant",
            Description = "Test Description",
            HasDelivery = true
        };

        //act
        var restaurant = _mapper.Map<Restaurant>(command);

        //assert
        restaurant.Should().NotBeNull();
        restaurant.Id.Should().Be(1);
        restaurant.Name.Should().Be("Test Restaurant");
        restaurant.Description.Should().Be("Test Description");
        restaurant.HasDelivery.Should().BeTrue();
    }
}