using FluentValidation;

namespace Application.Features.Projects.Command.DeleteProject
{
    public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
    {
        public DeleteProjectCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Project ID is required.");
        }
    }
}

