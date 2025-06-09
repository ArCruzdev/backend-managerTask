using MediatR;

namespace Application.Features.TaskItems.Commands.DeleteTaskItem;

public record DeleteTaskItemCommand(Guid Id) : IRequest<bool>;

