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
        // 1. Obtener la entidad TaskItem por su ID.
        // Incluimos Project y AssignedToUser para mapear sus nombres en el DTO.
        var taskItem = await _context.TaskItems
                                     .Include(ti => ti.Project) // Cargar la relación con Project
                                     .Include(ti => ti.AssignedToUser) // Cargar la relación con AssignedToUser
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(ti => ti.Id == request.Id, cancellationToken);

        // 2. Si no se encuentra la tarea, lanzar una excepción.
        if (taskItem == null)
        {
            throw new NotFoundException(nameof(TaskItem), request.Id); // NotFoundException se creará
        }

        // 3. Mapear la entidad a TaskItemDto.
        var taskItemDto = _mapper.Map<TaskItemDto>(taskItem);

        return taskItemDto;
    }
}
