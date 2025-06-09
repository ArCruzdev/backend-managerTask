using Application.Common.Mappings;
using Domain.Entities; 
namespace Application.Features.TaskItems.Queries;

public class CommentDto : IMapFrom<Comment>
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime Created { get; set; } 

    public Guid TaskItemId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty; 
}
