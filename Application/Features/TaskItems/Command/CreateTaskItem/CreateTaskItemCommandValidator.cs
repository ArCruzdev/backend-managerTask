using FluentValidation;

namespace Application.Features.TaskItems.Commands.CreateTaskItem;

public class CreateTaskItemCommandValidator : AbstractValidator<CreateTaskItemCommand>
{
    public CreateTaskItemCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título no puede estar vacío.")
            .MaximumLength(100).WithMessage("El título no puede exceder los 100 caracteres.")
            .Matches(@"^[a-zA-Z\s.,;:_!?()&'-]+$") 
            .WithMessage("El título solo puede contener letras, espacios y algunos caracteres especiales como . , ; : _ ! ? ( ) & ' -");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres."); 

        RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(DateTime.Today.Date) 
            .WithMessage("La fecha de vencimiento no puede ser anterior a hoy.");

        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("El ID del proyecto es requerido.");

        
    }
}
