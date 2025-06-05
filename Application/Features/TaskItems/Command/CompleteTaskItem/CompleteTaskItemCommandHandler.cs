using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Tasks.Commands.CompleteTaskItem;

public class CompleteTaskItemCommandHandler : IRequestHandler<CompleteTaskItemCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public CompleteTaskItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CompleteTaskItemCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.TaskItems
            .Include(t => t.Project)
                .ThenInclude(p => p.Tasks) // 👈 Necesario para verificar si el proyecto puede finalizarse
            .FirstOrDefaultAsync(t => t.Id == request.TaskItemId, cancellationToken);

        if (task == null)
            throw new NotFoundException(nameof(TaskItem), request.TaskItemId);

        task.Complete(); // Ejecuta la lógica de negocio: valida estado, registra fecha, y finaliza proyecto si corresponde

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
