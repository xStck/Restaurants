using MediatR;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.User.Commands.AssignUserRole;

public class AssignUserRoleCommand : IRequest
{
    public string UserEmail { get; set; } = default!;
    public string RoleName { get; set; } = default!;
}