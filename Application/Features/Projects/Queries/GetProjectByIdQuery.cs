using MediatR;

namespace Application.Features.Projects.Queries;

public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectDto>;

