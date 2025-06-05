using MediatR;

namespace Application.Features.Users.Command.CreateUser;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Username,
    string Role
) : IRequest<Guid>;

