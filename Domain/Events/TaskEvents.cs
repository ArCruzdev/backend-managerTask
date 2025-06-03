using Domain.Common;
using Domain.Entities;


namespace Domain.Events
{
    /// <summary>
    /// Event triggered when a new Task is created.
    /// </summary>
    public class TaskCreatedEvent : BaseEvent
    {
        public TaskItem Task { get; }

        public TaskCreatedEvent(TaskItem task)
        {
            Task = task;
        }
    }

    /// <summary>
    /// Event triggered when a Task's details are updated.
    /// </summary>
    public class TaskUpdatedEvent : BaseEvent
    {
        public TaskItem Task { get; }

        public TaskUpdatedEvent(TaskItem task)
        {
            Task = task;
        }
    }

    /// <summary>
    /// Event triggered when a Task is assigned to a user.
    /// </summary>
    public class TaskAssignedEvent : BaseEvent
    {
        public TaskItem Task { get; }
        public Guid AssignedToUserId { get; } 

        public TaskAssignedEvent(TaskItem task, Guid assignedToUserId)
        {
            Task = task;
            AssignedToUserId = assignedToUserId;
        }
    }

    /// <summary>
    /// Event triggered when a Task is unassigned from a user.
    /// </summary>
    public class TaskUnassignedEvent : BaseEvent
    {
        public TaskItem Task { get; }
        public Guid? PreviousAssignedToUserId { get; }

        public TaskUnassignedEvent(TaskItem task, Guid? previousAssignedToUserId)
        {
            Task = task;
            PreviousAssignedToUserId = previousAssignedToUserId;
        }
    }

    /// <summary>
    /// Event triggered when a Task's progress is started (changes from Pending to InProgress).
    /// </summary>
    public class TaskProgressStartedEvent : BaseEvent
    {
        public TaskItem Task { get; }

        public TaskProgressStartedEvent(TaskItem task)
        {
            Task = task;
        }
    }

    /// <summary>
    /// Event triggered when a Task is marked as completed.
    /// </summary>
    public class TaskCompletedEvent : BaseEvent
    {
        public TaskItem Task { get; }

        public TaskCompletedEvent(TaskItem task)
        {
            Task = task;
        }
    }

    /// <summary>
    /// Event triggered when a Task is canceled.
    /// </summary>
    public class TaskCanceledEvent : BaseEvent
    {
        public TaskItem Task { get; }

        public TaskCanceledEvent(TaskItem task)
        {
            Task = task;
        }
    }

    /// <summary>
    /// Evento disparado cuando se añade un comentario a un TaskItem.
    /// </summary>
    public class TaskCommentAddedEvent : BaseEvent
    {
        public TaskItem TaskItem { get; }
        public Guid CommentId { get; } // A menudo es suficiente con el ID del comentario

        public TaskCommentAddedEvent(TaskItem taskItem, Guid commentId)
        {
            TaskItem = taskItem;
            CommentId = commentId;
        }
    }
}
