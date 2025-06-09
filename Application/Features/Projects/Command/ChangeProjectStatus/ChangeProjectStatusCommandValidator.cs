using FluentValidation;
using Domain.Enums;

namespace Application.Features.Projects.Command.ChangeProjectStatus;

public class ChangeProjectStatusCommandValidator : AbstractValidator<ChangeProjectStatusCommand>
{
    public ChangeProjectStatusCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("ProjectId is required.");

        RuleFor(x => x.NewStatus)
            .IsInEnum().WithMessage("NewStatus must be a valid ProjectStatus value.");
    }
}

