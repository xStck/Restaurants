using FluentAssertions;
using Restaurants.Application.User;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Tests.UserTests;

public class CurrentUserTests
{
    [Theory]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRole_WithMatchingRole_ShouldReturnTrue(string role)
    {
        // arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        //act
        var isInRole = currentUser.IsInRole(role);

        //assert
        isInRole.Should().BeTrue();
    }

    [Fact]
    public void IsInRole_WithNonMatchingRole_ShouldReturnFalse()
    {
        //arrange
        var currentUser = new CurrentUser("2", "test2@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        //act
        var isInRole = currentUser.IsInRole(UserRoles.Owner);

        //assert
        isInRole.Should().BeFalse();
    }

    [Fact]
    public void IsInRole_WithNonMatchingRoleCawse_ShouldReturnFalse()
    {
        //arrange
        var currentUser = new CurrentUser("2", "test2@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        //act
        var isInRole = currentUser.IsInRole(UserRoles.Owner.ToLower());

        //assert
        isInRole.Should().BeFalse();
    }
}