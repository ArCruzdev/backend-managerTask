using MediatR;
using System;
using Domain.Enums; 

namespace Application.Features.TaskItems.Commands.CreateTaskItem;

// Un Comando representa una intención de cambiar el estado del sistema.
// Devolverá el Guid del TaskItem recién creado.
public record CreateTaskItemCommand : IRequest<Guid>
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime DueDate { get; init; }
    public Guid ProjectId { get; init; }
    public TaskPriority Priority { get; init; } = TaskPriority.Medium; 
    public Guid? AssignedToUserId { get; init; } 
}
