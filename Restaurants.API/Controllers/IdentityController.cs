using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.User.Commands.UpdateUserDetails;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController(IMediator mediatR) : ControllerBase
{
    [HttpPatch("user")]
    [Authorize]
    public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
    {
        await mediatR.Send(command);
        return NoContent();
    }
}