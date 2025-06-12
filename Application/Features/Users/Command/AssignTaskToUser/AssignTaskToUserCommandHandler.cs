using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Features.TaskItems.Commands.AssignTaskToUser;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Command.AssignTaskToUser;

public class AssignTaskToUserCommandHandler : IRequestHandler<AssignTaskToUserCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public AssignTaskToUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(AssignTaskToUserCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.TaskItems
            .Include(t => t.Project)
            .ThenInclude(p => p.Members)
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

        if (task == null)
            throw new NotFoundException(nameof(TaskItem), request.TaskId);

        var user = await _context.Users
            .Include(u => u.MemberOfProjects)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
            throw new NotFoundException(nameof(User), request.UserId);

        // Validar que el usuario sea miembro del proyecto de la tarea
        if (!user.MemberOfProjects.Any(p => p.Id == task.Project.Id))
            throw new InvalidUserOperationException("User is not a member of the project associated with the task.");

        // Validar que el usuario esté activo
        if (!user.IsActive)
            throw new InvalidUserOperationException("Cannot assign tasks to an inactive user.");

        // Asignar la tarea al usuario
        task.AssignToUser(request.UserId); 

        user.AssignTask(task); 

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
