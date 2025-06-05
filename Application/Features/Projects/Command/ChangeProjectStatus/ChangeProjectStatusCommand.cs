using Domain.Enums;
using MediatR;

namespace Application.Features.Projects.Command.ChangeProjectStatus;

public record ChangeProjectStatusCommand(Guid ProjectId, ProjectStatus NewStatus) : IRequest<bool>;

