using MediatR;
using System.Collections.Generic;

namespace Application.Features.TaskItems.Queries.GetTasksByProjectId;

public class GetTasksByProjectIdQuery : IRequest<List<TaskItemDto>>
{
    // El ID del proyecto que recibimos desde el frontend (que es string)
    public string ProjectId { get; set; }

    public GetTasksByProjectIdQuery(string projectId)
    {
        ProjectId = projectId;
    }
}
