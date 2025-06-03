using MediatR;
using Application.Common.Interfaces;
using Domain.Entities; 
using Application.Common.Exceptions; // Para NotFoundException si ProjectId o AssignedToUserId no existen
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TaskItems.Commands.CreateTaskItem;

public class CreateTaskItemCommandHandler : IRequestHandler<CreateTaskItemCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    // Inyectaremos ICurrentUserService más adelante para obtener el usuario actual
    // private readonly ICurrentUserService _currentUserService;

    public CreateTaskItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
        // _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar que el ProjectId exista (esto puede ser una regla de dominio o aplicación)
        var project = await _context.Projects.FindAsync(new object[] { request.ProjectId }, cancellationToken);
        if (project == null)
        {
            throw new NotFoundException(nameof(Project), request.ProjectId);
        }

        // 2. Validar que el AssignedToUserId exista si se proporciona
        User? assignedToUser = null;
        if (request.AssignedToUserId.HasValue)
        {
            assignedToUser = await _context.Users.FindAsync(new object[] { request.AssignedToUserId.Value }, cancellationToken);
            if (assignedToUser == null)
            {
                throw new NotFoundException(nameof(User), request.AssignedToUserId.Value);
            }
            // Aquí podríamos añadir lógica de negocio: ¿el usuario está activo? ¿Es miembro del proyecto?
            // Esto último implicaría cargar los miembros del proyecto o tener una relación más directa.
            // Por ahora, asumimos que solo necesita existir.
        }

        // 3. Crear la entidad de dominio. La lógica de negocio reside aquí.
        var entity = new TaskItem(
            request.Title,
            request.DueDate,
            request.ProjectId,
            request.Description,
            request.Priority
        );

        // Si se asignó un usuario al crear, llamar al método de dominio para asignarlo.
        if (assignedToUser != null)
        {
            entity.AssignToUser(assignedToUser);
        }

        // 4. Añadir la entidad al contexto y guardar cambios.
        _context.TaskItems.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        // 5. Devolver el ID del TaskItem recién creado.
        return entity.Id;
    }
}
