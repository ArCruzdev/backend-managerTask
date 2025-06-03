using MediatR;


namespace Application.Features.TaskItems.Queries;

// Esta Query tomará un Guid como parámetro (el ID del TaskItem)
// y devolverá un TaskItemDto.
public record GetTaskItemByIdQuery(Guid Id) : IRequest<TaskItemDto>;
