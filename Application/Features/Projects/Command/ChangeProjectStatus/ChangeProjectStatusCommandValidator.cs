using FluentValidation;
namespace Application.Features.Projects.Command.ChangeProjectStatus;

public class ChangeProjectStatusCommandValidator : AbstractValidator<ChangeProjectStatusCommand>
{
    public ChangeProjectStatusCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("El nombre del proyecto es requerido");
    }
}

