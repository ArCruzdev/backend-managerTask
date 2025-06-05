using FluentValidation;

namespace Application.Features.TaskItems.Commands.UpdateTaskItem;

public class UpdateTaskItemCommandValidator : AbstractValidator<UpdateTaskItemCommand>
{
    public UpdateTaskItemCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DueDate).GreaterThanOrEqualTo(DateTime.Today);
    }
}

