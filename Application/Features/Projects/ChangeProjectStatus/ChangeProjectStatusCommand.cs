using Domain.Enums;
using MediatR;

namespace Application.Features.Projects.Commands.ChangeProjectStatus;

public record ChangeProjectStatusCommand(Guid ProjectId, ProjectStatus NewStatus) : IRequest<bool>;

