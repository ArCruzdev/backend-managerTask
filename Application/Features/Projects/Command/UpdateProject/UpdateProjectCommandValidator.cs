using FluentValidation;

namespace Application.Features.Projects.Command.UpdateProject;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Project Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required.");

        RuleFor(x => x.EndDate)
            .Must((command, endDate) =>
                !endDate.HasValue || endDate.Value != default)
            .WithMessage("End date must be valid.");

        RuleFor(x => x.Budget)
            .GreaterThanOrEqualTo(0).When(x => x.Budget.HasValue)
            .WithMessage("Budget must be greater than or equal to zero.");
    }
}
