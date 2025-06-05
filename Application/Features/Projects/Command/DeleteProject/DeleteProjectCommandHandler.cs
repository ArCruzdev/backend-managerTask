using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Events;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Projects.Command.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public DeleteProjectCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks) // si necesitas validar dependencias
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (project == null)
            {
                throw new NotFoundException("Project", request.Id);
            }

            project.EnsureCanBeDeleted(); // si tienes una validación de dominio
            project.AddDomainEvent(new ProjectDeletedEvent(project));
            
            _context.Projects.Remove(project);

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}

