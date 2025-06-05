using MediatR;

namespace Application.Features.Projects.Command.UpdateProject;

public record UpdateProjectCommand(
    Guid Id,
    string Name,
    string? Description,
    DateTime? EndDate,
    decimal? Budget
) : IRequest<bool>;

