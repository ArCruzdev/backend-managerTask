using MediatR;
using System.Collections.Generic;

namespace Application.Features.TaskItems.Queries.GetTasksByProjectId;

public class GetTasksByProjectIdQuery : IRequest<List<TaskItemDto>>
{
    public string ProjectId { get; set; }

    public GetTasksByProjectIdQuery(string projectId)
    {
        ProjectId = projectId;
    }
}
