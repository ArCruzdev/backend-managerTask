using MediatR;
using Application.Common.Interfaces;
using Domain.Entities;
using Application.Common.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TaskItems.Commands.CreateTaskItem;

public class CreateTaskItemCommandHandler : IRequestHandler<CreateTaskItemCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateTaskItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar la existencia del proyecto.
        var project = await _context.Projects.FindAsync(new object[] { request.ProjectId }, cancellationToken);
        if (project == null)
        {
            throw new NotFoundException(nameof(Project), request.ProjectId);
        }

        // 2. Validar la existencia del usuario asignado (si se proporciona).
        User? assignedToUser = null;
        if (request.AssignedToUserId.HasValue)
        {
            assignedToUser = await _context.Users.FindAsync(new object[] { request.AssignedToUserId.Value }, cancellationToken);
            if (assignedToUser == null)
            {
                throw new NotFoundException(nameof(User), request.AssignedToUserId.Value);
            }
        }

        // 3. Crear la nueva entidad TaskItem.
        var entity = new TaskItem(
            request.Title,
            request.DueDate,
            request.ProjectId,
            request.Description,
            request.Priority,
            request.AssignedToUserId
        );

        // 4. Agregar la entidad al contexto de forma asincrónica.
        // CORRECCIÓN APLICADA: Usar AddAsync en lugar de Add.
        await _context.TaskItems.AddAsync(entity, cancellationToken);

        // 5. Guardar los cambios en la base de datos de forma asincrónica.
        await _context.SaveChangesAsync(cancellationToken);

        // 6. Retornar el ID de la nueva tarea.
        return entity.Id;
    }
}
