using MediatR;

namespace Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Username,
    string Role
) : IRequest<Guid>;

