using FluentValidation;

namespace Application.Features.Projects.Command.CreateProject;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .Must(date => date != default).WithMessage("Start date must be valid.");

        RuleFor(x => x.Budget)
            .GreaterThanOrEqualTo(0).When(x => x.Budget.HasValue)
            .WithMessage("Budget must be greater than or equal to zero.");
    }
}

