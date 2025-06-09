using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums; 
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TaskItems.Commands.UpdateTaskItem;

public class UpdateTaskItemCommandHandler : IRequestHandler<UpdateTaskItemCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateTaskItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateTaskItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TaskItems
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(TaskItem), request.Id);

        
        entity.UpdateDetails(
            request.Title,
            request.Description,
            request.DueDate,
            request.Status, 
            request.Priority,
            request.CompletionDate, 
            request.AssignedToUserId 
        );

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
