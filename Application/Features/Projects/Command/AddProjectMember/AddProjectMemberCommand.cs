using MediatR;

namespace Application.Features.Projects.Command.AddProjectMember;

public record AddProjectMemberCommand(Guid ProjectId, Guid UserId) : IRequest<bool>;

