using FluentValidation.TestHelper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Validators;

namespace Restaurants.Application.Tests.ValidatorsTests;

public class CreateRestaurantCommandValidatorTests
{
    [Fact]
    public void Validator_ForValidCommand_ShouldNotHaveValidatorErrors()
    {
        //arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Test",
            Category = "Italian",
            ContactEmail = "test@test.com",
            PostalCode = "11-111"
        };

        var validator = new CreateRestaurantCommandValidator();

        //act
        var result = validator.TestValidate(command);

        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
    {
        //arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Te",
            Category = "Argentina",
            ContactEmail = "testtest.com",
            PostalCode = "11111"
        };

        var validator = new CreateRestaurantCommandValidator();

        //act
        var result = validator.TestValidate(command);

        //assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
        result.ShouldHaveValidationErrorFor(c => c.Category);
        result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }

    [Theory]
    [InlineData("Italian")]
    [InlineData("Mexican")]
    [InlineData("Japanese")]
    [InlineData("American")]
    [InlineData("Polish")]
    [InlineData("Indian")]
    public void Validator_ForValidCategory_ShouldNotHaveValidationErrors(string category)
    {
        //arrange
        var command = new CreateRestaurantCommand()
        {
            Category = category,
        };
        var validator = new CreateRestaurantCommandValidator();
        //act
        var result = validator.TestValidate(command);

        //assert
        result.ShouldNotHaveValidationErrorFor(c => c.Category);
    }

    [Theory]
    [InlineData("111111")]
    [InlineData("111-11")]
    [InlineData("111 11")]
    [InlineData("11-1 11")]
    public void Validator_ForInvalidPostalCode_ShouldHaveValidationErrors(string postalCode)
    {
        //arrange
        var command = new CreateRestaurantCommand()
        {
            PostalCode = postalCode
        };
        var validator = new CreateRestaurantCommandValidator();

        //act
        var result = validator.TestValidate(command);

        //assert
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }
}