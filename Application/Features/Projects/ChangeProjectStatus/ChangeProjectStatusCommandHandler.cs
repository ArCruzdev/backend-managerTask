using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Projects.Commands.ChangeProjectStatus;

public class ChangeProjectStatusCommandHandler : IRequestHandler<ChangeProjectStatusCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public ChangeProjectStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ChangeProjectStatusCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

        if (project == null)
            throw new NotFoundException(nameof(Project), request.ProjectId);

        project.ChangeStatus(request.NewStatus);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

