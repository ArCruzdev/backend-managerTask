using MediatR;

namespace Application.Features.Projects.Command.DeleteProject
{
    public record DeleteProjectCommand(Guid Id) : IRequest<bool>;
}
