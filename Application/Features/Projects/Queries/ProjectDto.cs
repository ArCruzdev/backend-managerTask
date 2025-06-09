using Application.Common.Mappings; 
using Domain.Entities;

namespace Application.Features.Projects.Queries;

public class ProjectDto : IMapFrom<Project> 
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = string.Empty; // Mapearemos el enum a string
    public bool HasActiveTasks { get; set; }

   
}
