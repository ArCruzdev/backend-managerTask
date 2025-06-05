using FluentValidation;

namespace Application.Features.Tasks.Commands.CompleteTaskItem;

public class CompleteTaskItemCommandValidator : AbstractValidator<CompleteTaskItemCommand>
{
    public CompleteTaskItemCommandValidator()
    {
        RuleFor(x => x.TaskItemId)
            .NotEmpty().WithMessage("TaskItemId is required.");
    }
}

