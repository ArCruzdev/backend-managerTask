// Path: Application/Features/TaskItems/Queries/CommentDto.cs
using Application.Common.Mappings;
using Domain.Entities; // Necesitamos acceso a la entidad Comment

namespace Application.Features.TaskItems.Queries;

public class CommentDto : IMapFrom<Comment>
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime Created { get; set; } // Propiedad de BaseAuditableEntity

    public Guid TaskItemId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty; // Para mostrar el nombre del autor del comentario
}
