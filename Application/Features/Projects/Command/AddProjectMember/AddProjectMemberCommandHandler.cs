using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Projects.Command.AddProjectMember;

public class AddProjectMemberCommandHandler : IRequestHandler<AddProjectMemberCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public AddProjectMemberCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(AddProjectMemberCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

        if (project == null)
            throw new NotFoundException(nameof(Project), request.ProjectId);

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
            throw new NotFoundException(nameof(User), request.UserId);

        project.AddMember(user);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

