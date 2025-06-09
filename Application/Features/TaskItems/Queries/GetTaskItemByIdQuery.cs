using MediatR;


namespace Application.Features.TaskItems.Queries;

public record GetTaskItemByIdQuery(Guid Id) : IRequest<TaskItemDto>;
