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
      
        var project = await _context.Projects.FindAsync(new object[] { request.ProjectId }, cancellationToken);
        if (project == null)
        {
            throw new NotFoundException(nameof(Project), request.ProjectId);
        }

        User? assignedToUser = null;
        if (request.AssignedToUserId.HasValue)
        {
            assignedToUser = await _context.Users.FindAsync(new object[] { request.AssignedToUserId.Value }, cancellationToken);
            if (assignedToUser == null)
            {
                throw new NotFoundException(nameof(User), request.AssignedToUserId.Value);
            }
            
        }
        var entity = new TaskItem(
            request.Title,
            request.DueDate,
            request.ProjectId,
            request.Description,
            request.Priority,
            request.AssignedToUserId
        );
        _context.TaskItems.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        
        return entity.Id;
    }
}
