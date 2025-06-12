using AutoMapper;
using MediatR;
using Application.Common.Interfaces;
using Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Domain.Entities; 

namespace Application.Features.TaskItems.Queries;

public class GetTaskItemByIdQueryHandler : IRequestHandler<GetTaskItemByIdQuery, TaskItemDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTaskItemByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TaskItemDto> Handle(GetTaskItemByIdQuery request, CancellationToken cancellationToken)
    { 
        var taskItem = await _context.TaskItems
                                     .Include(ti => ti.Project) 
                                     .Include(ti => ti.AssignedToUser) 
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(ti => ti.Id == request.Id, cancellationToken);
        if (taskItem == null)
        {
            throw new NotFoundException(nameof(TaskItem), request.Id);
        }
        var taskItemDto = _mapper.Map<TaskItemDto>(taskItem);

        return taskItemDto;
    }
}
