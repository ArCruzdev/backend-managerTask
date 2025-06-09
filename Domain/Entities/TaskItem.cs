// Domain/Entities/TaskItem.cs
using Domain.Common;
using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using System; // Agregado para DateTime y Guid

namespace Domain.Entities
{
    public class TaskItem : BaseAuditableEntity
    {
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public DateTime DueDate { get; private set; }
        public TaskItemStatus Status { get; private set; } // Propiedad del estado
        public TaskPriority Priority { get; private set; }
        public DateTime? CompletionDate { get; private set; } // Propiedad de fecha de completado

        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; } = null!; // Navegación

        public Guid? AssignedToUserId { get; private set; } // Propiedad del usuario asignado
        public User? AssignedToUser { get; private set; } 

        private readonly List<Comment> _comments = new();
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

        // Constructor privado para EF Core
        private TaskItem() { }

        // Constructor público para creación de nuevas tareas
        public TaskItem(string title, DateTime dueDate, Guid projectId, string? description = null, TaskPriority priority = TaskPriority.Medium, Guid? assignedToUserId = null)
        {
            // Validaciones para la creación
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException(nameof(title), "Task title cannot be empty.");
            }
            
            
            if (dueDate == default || dueDate.Date < DateTime.Today.Date)
            {
                throw new ArgumentException("Due date must be a valid date in the future or today.", nameof(dueDate));
            }
            if (projectId == Guid.Empty)
            {
                throw new ArgumentException("Task must belong to a valid project.", nameof(projectId));
            }

            Title = title;
            Description = description;
            DueDate = dueDate;
            ProjectId = projectId;
            Status = TaskItemStatus.Pending; 
            Priority = priority;
            AssignedToUserId = assignedToUserId; 

            AddDomainEvent(new TaskCreatedEvent(this));
        }

        // --- Business Logic Methods ---

        /// <summary>
        /// Updates the task's core details including status, priority, and assigned user.
        /// Este es el método que tu UpdateTaskItemCommand debería usar.
        /// </summary>
        public void UpdateDetails(
            string newTitle,
            string? newDescription,
            DateTime newDueDate,
            TaskItemStatus newStatus, 
            TaskPriority newPriority,
            DateTime? newCompletionDate, 
            Guid? newAssignedToUserId 
        )
        {
            // Validaciones para la actualización 
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                throw new ArgumentNullException(nameof(newTitle), "Task title cannot be empty.");
            }
            if (newDueDate == default || newDueDate.Date < DateTime.Today.Date)
            {
                throw new ArgumentException("Due date must be a valid date in the future or today.", nameof(newDueDate));
            }

            Title = newTitle;
            Description = newDescription;
            DueDate = newDueDate;
            Status = newStatus; 
            Priority = newPriority;
            CompletionDate = newCompletionDate; 
            AssignedToUserId = newAssignedToUserId; 

            
            if (newStatus == TaskItemStatus.Completed && !CompletionDate.HasValue)
            {
                CompletionDate = DateTime.UtcNow;
            }
            
            if (newStatus != TaskItemStatus.Completed && CompletionDate.HasValue)
            {
                CompletionDate = null;
            }


            AddDomainEvent(new TaskUpdatedEvent(this));
        }

        
        /// <summary>
        /// Changes the status of the task. Includes business rules for transitions.
        /// </summary>
        public void ChangeStatus(TaskItemStatus newStatus)
        {
            if (Status == newStatus) return; // No cambia si es el mismo estado

            switch (newStatus)
            {
                case TaskItemStatus.Pending:
                    if (Status == TaskItemStatus.Completed)
                    {
                        throw new InvalidTaskOperationException("Cannot revert a completed task to pending.");
                    }
                    Status = TaskItemStatus.Pending;
                    CompletionDate = null; 
                    break;
                case TaskItemStatus.InProgress:
                    if (Status == TaskItemStatus.Completed || Status == TaskItemStatus.Canceled)
                    {
                        throw new InvalidTaskOperationException("Cannot start progress on a completed or canceled task.");
                    }
                    Status = TaskItemStatus.InProgress;
                    CompletionDate = null;
                    break;
                case TaskItemStatus.Completed:
                    if (Status == TaskItemStatus.Canceled)
                    {
                        throw new InvalidTaskOperationException("Cannot complete a canceled task.");
                    }
                    Status = TaskItemStatus.Completed;
                    CompletionDate = DateTime.UtcNow; 
                    Project?.FinalizeIfAllTasksCompleted(); 
                    break;
                case TaskItemStatus.Canceled:
                    if (Status == TaskItemStatus.Completed)
                    {
                        throw new InvalidTaskOperationException("Cannot cancel a completed task.");
                    }
                    Status = TaskItemStatus.Canceled;
                    CompletionDate = null; 
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newStatus), "Invalid task status.");
            }
            AddDomainEvent(new TaskStatusChangedEvent(this, Status)); 
        }

        /// <summary>
        /// Assigns the task to a user.
        /// </summary>
        /// <param name="userId">The ID of the user to assign. Can be null to unassign.</param>
        public void AssignToUser(Guid? userId)
        {
            if (AssignedToUserId == userId) return;

            
            if (userId.HasValue)
            {
                AssignedToUserId = userId.Value; 
                                                 
                if (Status == TaskItemStatus.Pending)
                {
                    Status = TaskItemStatus.InProgress;
                }
                
                AddDomainEvent(new TaskAssignedEvent(this, userId.Value)); // <--- CAMBIO AQUÍ: userId.Value
            }
            else 
            {
                if (AssignedToUserId.HasValue) 
                {
                    Guid? previousUserId = AssignedToUserId; 
                    AssignedToUserId = null;
                    
                    if (Status == TaskItemStatus.InProgress)
                    {
                        Status = TaskItemStatus.Pending;
                    }
                    
                    AddDomainEvent(new TaskUnassignedEvent(this, previousUserId.Value)); // <--- Asumiendo que TaskUnassignedEvent espera Guid
                }
            }

            
            
        }
        /// <summary>
        /// Adds a comment to the task.
        /// </summary>
        public void AddComment(Comment comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment));
            }
            if (comment.TaskItemId != Id)
            {
                throw new InvalidOperationException("Comment is not associated with this task.");
            }

            _comments.Add(comment);
            AddDomainEvent(new TaskCommentAddedEvent(this, comment.Id));
        }

        /// <summary>
        /// Ensures the task can be deleted based on its status.
        /// </summary>
        public void EnsureCanBeDeleted()
        {
            if (Status == TaskItemStatus.Completed || Status == TaskItemStatus.Canceled)
            {
                throw new InvalidTaskOperationException("Cannot delete a task that is completed or canceled.");
            }
        }
    }
}