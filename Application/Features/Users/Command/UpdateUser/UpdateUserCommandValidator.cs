using FluentValidation;

namespace Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("User ID is required.");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.").MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.").MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").EmailAddress().WithMessage("Invalid email format.");
    }
}

