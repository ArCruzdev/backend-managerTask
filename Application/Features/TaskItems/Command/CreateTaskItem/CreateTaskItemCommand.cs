using MediatR;
using Domain.Enums; 

namespace Application.Features.TaskItems.Commands.CreateTaskItem;

public record CreateTaskItemCommand : IRequest<Guid>
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime DueDate { get; init; }
    public Guid ProjectId { get; init; }
    public TaskPriority Priority { get; init; } = TaskPriority.Medium; 
    public Guid? AssignedToUserId { get; init; } 
}
