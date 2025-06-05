using FluentValidation;

namespace Application.Features.TaskItems.Commands.CreateTaskItem;

public class CreateTaskItemCommandValidator : AbstractValidator<CreateTaskItemCommand>
{
    public CreateTaskItemCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DueDate).GreaterThanOrEqualTo(DateTime.Today);
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}
