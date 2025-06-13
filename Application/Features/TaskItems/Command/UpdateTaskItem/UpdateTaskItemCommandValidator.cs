using Domain.Enums; 
using FluentValidation;
using Application.Features.TaskItems.Commands.UpdateTaskItem;

public class UpdateTaskItemCommandValidator : AbstractValidator<UpdateTaskItemCommand>
{
    public UpdateTaskItemCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("El título es requerido.")
            .MaximumLength(200).WithMessage("El título no puede exceder los 200 caracteres.");

        RuleFor(v => v.DueDate)
            .NotNull().When(v => v.Status != TaskItemStatus.Completed && v.Status != TaskItemStatus.Canceled)
            .WithMessage("La fecha de vencimiento es requerida para tareas activas.");

        RuleFor(v => v.DueDate)
            .Must((command, dueDate) =>
            {
                if (command.Status == TaskItemStatus.Completed || command.Status == TaskItemStatus.Canceled)
                {
                    return true;
                }

                return dueDate.Date >= DateTime.UtcNow.Date;
            })
            .WithMessage("La fecha de vencimiento no puede ser anterior a hoy para tareas activas.");

        RuleFor(v => v.CompletionDate)
            .Must((command, completionDate) =>
            {
                
                if (command.Status == TaskItemStatus.Completed && !completionDate.HasValue) return false;
                
                if (command.Status != TaskItemStatus.Completed && completionDate.HasValue) return false;
                return true;
            })
            .WithMessage("La fecha de completado es requerida si el estado es completado, y debe ser nula si no lo es.");
        
    }
}

