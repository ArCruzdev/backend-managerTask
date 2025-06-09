using MediatR;

namespace Application.Features.TaskItems.Commands.AssignTaskToUser;

public record AssignTaskToUserCommand(Guid TaskId, Guid UserId) : IRequest<bool>;
