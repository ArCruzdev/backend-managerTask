using MediatR;

namespace Application.Features.Tasks.Commands.CompleteTaskItem;

public record CompleteTaskItemCommand(Guid TaskItemId) : IRequest<bool>;
