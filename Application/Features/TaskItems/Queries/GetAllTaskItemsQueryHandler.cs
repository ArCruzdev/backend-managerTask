using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TaskItems.Queries;

public class GetAllTaskItemsQueryHandler : IRequestHandler<GetAllTaskItemsQuery, List<TaskItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllTaskItemsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<TaskItemDto>> Handle(GetAllTaskItemsQuery request, CancellationToken cancellationToken)
    {
        return await _context.TaskItems
            .Include(t => t.Project)
            .Include(t => t.AssignedToUser)
            .AsNoTracking()
            .ProjectTo<TaskItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}

