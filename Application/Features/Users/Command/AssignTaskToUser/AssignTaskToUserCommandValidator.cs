using FluentValidation;

namespace Application.Features.TaskItems.Commands.AssignTaskToUser;

public class AssignTaskToUserCommandValidator : AbstractValidator<AssignTaskToUserCommand>
{
    public AssignTaskToUserCommandValidator()
    {
        RuleFor(x => x.TaskId).NotEmpty().WithMessage("Task ID is required.");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
    }
}

