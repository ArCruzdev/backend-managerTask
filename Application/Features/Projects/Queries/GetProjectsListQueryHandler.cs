using AutoMapper;
using MediatR;
using Application.Common.Interfaces; 
using Domain.Entities; 
using Microsoft.EntityFrameworkCore; 

namespace Application.Features.Projects.Queries;


public class GetProjectsListQueryHandler : IRequestHandler<GetProjectsListQuery, List<ProjectDto>>
{
    private readonly IApplicationDbContext _context; 
    private readonly IMapper _mapper; 
    public GetProjectsListQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

 
    public async Task<List<ProjectDto>> Handle(GetProjectsListQuery request, CancellationToken cancellationToken)
    {
        var projects = await _context.Projects
                                     .AsNoTracking() 
                                     .OrderBy(p => p.Name) 
                                     .ToListAsync(cancellationToken);
        var projectDtos = _mapper.Map<List<ProjectDto>>(projects);

        return projectDtos;
    }
}
