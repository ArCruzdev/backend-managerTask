using FluentValidation;

namespace Application.Features.TaskItems.Commands.DeleteTaskItem;

public class DeleteTaskItemCommandValidator : AbstractValidator<DeleteTaskItemCommand>
{
    public DeleteTaskItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("TaskItem Id is required.");
    }
}

