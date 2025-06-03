using Domain.Common;
using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;


namespace Domain.Entities
{
    public class TaskItem : BaseAuditableEntity
    {
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public DateTime DueDate { get; private set; }
        public TaskItemStatus Status { get; private set; }
        public TaskPriority Priority { get; private set; }
        public DateTime? CompletionDate { get; private set; } 

        
        public Guid ProjectId { get; private set; }
        
        public Project Project { get; private set; } = null!; 

        
        public Guid? AssignedToUserId { get; private set; }
       
        public User? AssignedToUser { get; private set; }

        
        private readonly List<Comment> _comments = new();
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

        
        private TaskItem() { }

        
        public TaskItem(string title, DateTime dueDate, Guid projectId, string? description = null, TaskPriority priority = TaskPriority.Medium)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException(nameof(title), "Task title cannot be empty.");
            }
            if (dueDate == default || dueDate < DateTime.Today.AddDays(-1)) 
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

            AddDomainEvent(new TaskCreatedEvent(this)); 
        }

        // --- Business Logic Methods ---

        /// <summary>
        /// Updates the task's details.
        /// </summary>
        public void UpdateDetails(string newTitle, string? newDescription, DateTime newDueDate, TaskPriority newPriority)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                throw new ArgumentNullException(nameof(newTitle), "Task title cannot be empty.");
            }
            if (newDueDate == default || newDueDate < DateTime.Today.AddDays(-1))
            {
                throw new ArgumentException("Due date must be a valid date in the future or today.", nameof(newDueDate));
            }

            Title = newTitle;
            Description = newDescription;
            DueDate = newDueDate;
            Priority = newPriority;

            AddDomainEvent(new TaskUpdatedEvent(this)); 
        }

        /// <summary>
        /// Assigns the task to a user.
        /// Business Rule: User must be active and a member of the project the task belongs to.
        /// </summary>
        public void AssignToUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }
            if (!user.IsActive)
            {
                throw new InvalidTaskOperationException($"Cannot assign task to an inactive user: {user.Email}.");
            }
            
            if (AssignedToUserId == user.Id)
            {
                return; 
            }

            AssignedToUserId = user.Id;
            AssignedToUser = user; 
            AddDomainEvent(new TaskAssignedEvent(this, user.Id)); 
        }

        /// <summary>
        /// Unassigns the task from its current user.
        /// </summary>
        public void UnassignUser()
        {
            if (AssignedToUserId == null)
            {
                return; 
            }

            Guid? previousUserId = AssignedToUserId;
            AssignedToUserId = null;
            AssignedToUser = null; 

            AddDomainEvent(new TaskUnassignedEvent(this, previousUserId)); 
        }

        /// <summary>
        /// Marks the task as in progress.
        /// Business Rule: Only a Pending task can transition to InProgress.
        /// </summary>
        public void StartProgress()
        {
            if (Status == TaskItemStatus.Completed || Status == TaskItemStatus.Canceled)
            {
                throw new InvalidTaskOperationException("Cannot start progress on a completed or canceled task.");
            }
            if (Status == TaskItemStatus.InProgress)
            {
                return; 
            }

            Status = TaskItemStatus.InProgress;
            AddDomainEvent(new TaskProgressStartedEvent(this)); 
        }

        /// <summary>
        /// Marks the task as completed.
        /// Business Rule: Only a Pending or InProgress task can transition to Completed.
        /// Also, sets the completion date.
        /// </summary>
        public void Complete()
        {
            if (Status == TaskItemStatus.Completed)
            {
                return; 
            }
            if (Status == TaskItemStatus.Canceled)
            {
                throw new InvalidTaskOperationException("Cannot complete a canceled task.");
            }

            Status = TaskItemStatus.Completed;
            CompletionDate = DateTime.UtcNow; 
            AddDomainEvent(new TaskCompletedEvent(this)); 
        }

        /// <summary>
        /// Marks the task as canceled.
        /// Business Rule: A completed task cannot be canceled.
        /// </summary>
        public void Cancel()
        {
            if (Status == TaskItemStatus.Canceled)
            {
                return; 
            }
            if (Status == TaskItemStatus.Completed)
            {
                throw new InvalidTaskOperationException("Cannot cancel a completed task.");
            }

            Status = TaskItemStatus.Canceled;
            AddDomainEvent(new TaskCanceledEvent(this)); 
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
    }
}
