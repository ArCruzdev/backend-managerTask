// Application/Features/Projects/Command/DeleteProject/DeleteProjectCommandHandler.cs
using Application.Common.Exceptions; // Importa tu BadRequestException y NotFoundException
using Application.Common.Interfaces;
using Domain.Events;
using Domain.Exceptions; // Si tienes excepciones de dominio aquí
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums; // <--- ¡Importante! Asegúrate de que esto apunte a tu enum TaskStatus real

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
                .Include(p => p.Tasks) // ¡Es crucial incluir las tareas para poder verificarlas!
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (project == null)
            {
                // Si el proyecto no se encuentra en la DB, lanza NotFoundException
                throw new NotFoundException("Project", request.Id);
            }

            // --- LÓGICA DE NEGOCIO: NO ELIMINAR SI HAY TAREAS ACTIVAS ---
            // Define aquí qué estados de tarea consideras "activos".
            // **AJUSTA ESTOS ESTADOS SEGÚN LA DEFINICIÓN EXACTA DE TU ENUM Domain.Enums.TaskStatus.**
            var activeTaskStatuses = new[]
            {
                TaskItemStatus.Pending,    // Ejemplo: Tarea pendiente
                TaskItemStatus.InProgress, // Ejemplo: Tarea en progreso
                // Si tienes otros estados que consideres activos (ej. 'Reopened', 'OnHold'), añádelos aquí.
            };

            // Verifica si hay ALGUNA tarea cuyo estado esté dentro de los "estados activos" definidos.
            bool hasActiveTasks = project.Tasks.Any(t => activeTaskStatuses.Contains(t.Status));

            if (hasActiveTasks)
            {
                // Si hay tareas activas, lanza una BadRequestException con el mensaje de negocio.
                throw new BadRequestException("No se puede eliminar este proyecto. Por favor, asegúrate de que todas las tareas estén completadas o inactivas antes de eliminar el proyecto.");
            }
            // --- FIN LÓGICA DE NEGOCIO ---


            // Esta es tu validación de dominio si la tienes implementada.
            // Asegúrate de que EnsureCanBeDeleted() no duplique la lógica de tareas activas si no es necesario.
            project.EnsureCanBeDeleted();
            project.AddDomainEvent(new ProjectDeletedEvent(project));

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}