using Domain.Common;
using Domain.Entities;

namespace Domain.Events
{
    /// <summary>
    /// Event triggered when a new Comment is created.
    /// </summary>
    public class CommentCreatedEvent : BaseEvent
    {
        public Comment Comment { get; }

        public CommentCreatedEvent(Comment comment)
        {
            Comment = comment;
        }
    }

    /// <summary>
    /// Event triggered when a Comment's text is updated.
    /// </summary>
    public class CommentUpdatedEvent : BaseEvent
    {
        public Comment Comment { get; }

        public CommentUpdatedEvent(Comment comment)
        {
            Comment = comment;
        }
    }
}
