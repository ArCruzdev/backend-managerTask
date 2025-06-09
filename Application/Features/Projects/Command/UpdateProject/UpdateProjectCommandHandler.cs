using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Projects.Command.UpdateProject;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (project == null)
            throw new NotFoundException(nameof(Project), request.Id);

        project.UpdateDetails(request.Name, request.Description, request.EndDate, request.Budget);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

