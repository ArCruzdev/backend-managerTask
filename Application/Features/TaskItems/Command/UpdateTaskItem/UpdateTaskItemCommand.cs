using MediatR;
using Domain.Enums; 
using System; 

namespace Application.Features.TaskItems.Commands.UpdateTaskItem;

public record UpdateTaskItemCommand : IRequest<bool>
{
    public Guid Id { get; init; }
    public Guid ProjectId { get; init; } 
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime DueDate { get; init; }
    public TaskItemStatus Status { get; init; } 
    public TaskPriority Priority { get; init; }
    public DateTime? CompletionDate { get; init; } 
    public Guid? AssignedToUserId { get; init; } 
}
