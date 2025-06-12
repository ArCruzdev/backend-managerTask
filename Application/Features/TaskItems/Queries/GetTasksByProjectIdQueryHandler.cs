using Application.Common.Interfaces; 
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Application.Features.TaskItems.Queries.GetTasksByProjectId;

public class GetTasksByProjectIdQueryHandler : IRequestHandler<GetTasksByProjectIdQuery, List<TaskItemDto>>
{
    private readonly IApplicationDbContext _context; 
    private readonly IMapper _mapper;

    public GetTasksByProjectIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<TaskItemDto>> Handle(GetTasksByProjectIdQuery request, CancellationToken cancellationToken)
    {
        
        if (!Guid.TryParse(request.ProjectId, out Guid projectIdGuid))
        {
            throw new ArgumentException("El ID del proyecto proporcionado no es un formato GUID válido.");
        }

        var tasks = await _context.TaskItems
                                  .Include(t => t.Project)       
                                  .Include(t => t.AssignedToUser)                            
                                  .Where(t => t.ProjectId == projectIdGuid)
                                  .OrderByDescending(t => t.Created) 
                                  .AsNoTracking() 
                                  .ToListAsync(cancellationToken);

        return _mapper.Map<List<TaskItemDto>>(tasks);
    }
}
