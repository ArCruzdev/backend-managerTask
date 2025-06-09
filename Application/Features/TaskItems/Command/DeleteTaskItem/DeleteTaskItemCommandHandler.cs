using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TaskItems.Commands.DeleteTaskItem;

public class DeleteTaskItemCommandHandler : IRequestHandler<DeleteTaskItemCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteTaskItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteTaskItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TaskItems
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(TaskItem), request.Id);

        _context.TaskItems.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        entity.EnsureCanBeDeleted();
        _context.TaskItems.Remove(entity);

        return true;
    }
}
