using MediatR;

namespace Application.Features.Projects.Queries;


public record GetProjectsListQuery : IRequest<List<ProjectDto>>
{
}