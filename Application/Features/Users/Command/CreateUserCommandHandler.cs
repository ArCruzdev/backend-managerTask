using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Username,
            request.Role
        );

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}

