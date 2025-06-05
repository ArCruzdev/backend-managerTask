using MediatR;
using Domain.Enums;

namespace Application.Features.TaskItems.Commands.UpdateTaskItem;

public record UpdateTaskItemCommand : IRequest<bool>
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime DueDate { get; init; }
    public TaskPriority Priority { get; init; }
}
