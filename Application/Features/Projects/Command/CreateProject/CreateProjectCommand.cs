using MediatR;

namespace Application.Features.Projects.Command.CreateProject;

public record CreateProjectCommand(string Name, DateTime StartDate, string? Description = null, decimal? Budget = null) : IRequest<Guid>;

