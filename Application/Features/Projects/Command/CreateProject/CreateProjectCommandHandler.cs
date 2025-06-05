using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Projects.Command.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project(request.Name, request.StartDate, request.Description, request.Budget);
        _context.Projects.Add(project);
        await _context.SaveChangesAsync(cancellationToken);
        return project.Id;
    }
}

