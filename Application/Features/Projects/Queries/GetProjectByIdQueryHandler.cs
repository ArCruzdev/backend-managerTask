using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Projects.Queries;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProjectByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.Members)
            .Include(p => p.Tasks)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (project == null)
            throw new NotFoundException(nameof(Project), request.Id);

        return _mapper.Map<ProjectDto>(project);
    }
}

