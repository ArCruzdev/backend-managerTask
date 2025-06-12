using Application.Common.Exceptions; 
using Application.Common.Interfaces;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums; 

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
                .Include(p => p.Tasks) 
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (project == null)
            {
                //If the project is not found in the DB, throw NotFoundException
                throw new NotFoundException("Project", request.Id);
            }

            // Does not delete if there are active tasks
            var activeTaskStatuses = new[]
            {
                TaskItemStatus.Pending,    
                TaskItemStatus.InProgress
            };

            // Check if there is ANY task whose status is within the defined "active states".
            bool hasActiveTasks = project.Tasks.Any(t => activeTaskStatuses.Contains(t.Status));

            if (hasActiveTasks)
            {
                // If there are active tasks, throw a BadRequestException with the business message.
                throw new BadRequestException("No se puede eliminar este proyecto. Por favor, asegúrate de que todas las tareas estén completadas o inactivas antes de eliminar el proyecto.");
            }
            
            project.EnsureCanBeDeleted();
            project.AddDomainEvent(new ProjectDeletedEvent(project));

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}