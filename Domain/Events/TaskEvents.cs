using Domain.Common;
using Domain.Entities;
using Domain.Enums; 

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
    /// This is a general event for broad updates (title, description, due date, priority).
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
        public Guid PreviousAssignedToUserId { get; } 

        public TaskUnassignedEvent(TaskItem task, Guid previousAssignedToUserId) 
        {
            Task = task;
            PreviousAssignedToUserId = previousAssignedToUserId;
        }
    }

    
    /// <summary>
    /// Event triggered when a Task's status changes. This replaces specific status events
    /// like TaskProgressStartedEvent, TaskCompletedEvent, TaskCanceledEvent if a single
    /// 'ChangeStatus' method is used.
    /// </summary>
    public class TaskStatusChangedEvent : BaseEvent
    {
        public TaskItem Task { get; }
        public TaskItemStatus NewStatus { get; } // El nuevo estado de la tarea

        public TaskStatusChangedEvent(TaskItem task, TaskItemStatus newStatus)
        {
            Task = task;
            NewStatus = newStatus;
        }
    }

    /// <summary>
    /// Event triggered when a comment is added to a TaskItem.
    /// </summary>
    public class TaskCommentAddedEvent : BaseEvent
    {
        public TaskItem TaskItem { get; }
        public Guid CommentId { get; }

        public TaskCommentAddedEvent(TaskItem taskItem, Guid commentId)
        {
            TaskItem = taskItem;
            CommentId = commentId;
        }
    }
}
