using Application.Common.Mappings;
using Domain.Entities; 

namespace Application.Features.TaskItems.Queries;

public class TaskItemDto : IMapFrom<TaskItem>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = string.Empty; // Mapear el enum a string
    public string Priority { get; set; } = string.Empty; // Mapear el enum a string
    public DateTime? CompletionDate { get; set; }

    // Propiedades relacionadas (solo IDs o DTOs planos para evitar ciclos)
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty; // Para mostrar el nombre del proyecto

    public Guid? AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; } // Para mostrar el nombre del usuario asignado
}
