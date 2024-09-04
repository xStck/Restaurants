using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.User;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Tests.UserTests;

public class UserContextTests
{
    [Fact]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        //arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var dateOfBirth = new DateOnly(1990, 1, 1);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "test@test.com"),
            new(ClaimTypes.Role, UserRoles.Admin),
            new(ClaimTypes.Role, UserRoles.User),
            new("Nationality", "Polish"),
            new("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"))
        };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
        httpContextAccessorMock
            .Setup(x => x.HttpContext).Returns(new DefaultHttpContext
            {
                User = user
            });
        var userContext = new UserContext(httpContextAccessorMock.Object);

        //act
        var currentUser = userContext.GetCurrentUser();

        //assert
        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@test.com");
        currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
        currentUser.Nationality.Should().Be("Polish");
        currentUser.DateOfBirth.Should().Be(dateOfBirth);
    }

    [Fact]
    public void GetCurrentUser_WithCurrentUserNotPresent_ThrowsInvalidOperationException()
    {
        //arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock
            .Setup(x => x.HttpContext).Returns((HttpContext)null);
        var userContext = new UserContext(httpContextAccessorMock.Object);

        //act
        var action = () => userContext.GetCurrentUser();
        //assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("User context is not present");
    }
}