using Domain.Common;
using Domain.Events;

namespace Domain.Entities
{
    public class Comment : BaseAuditableEntity
    {
        public string Text { get; private set; }

        
        public Guid TaskItemId { get; private set; }
        
        public TaskItem TaskItem { get; private set; } = null!; 

        
        public Guid UserId { get; private set; }
        
        public User User { get; private set; } = null!; 

        
        private Comment() { }

        // Public constructor for creating a new comment
        public Comment(string text, Guid taskId, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(nameof(text), "Comment text cannot be empty.");
            }
            if (taskId == Guid.Empty)
            {
                throw new ArgumentException("Comment must be associated with a valid task.", nameof(taskId));
            }
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("Comment must be made by a valid user.", nameof(userId));
            }

            Text = text;
            TaskItemId = taskId;
            UserId = userId;

            AddDomainEvent(new CommentCreatedEvent(this)); 
        }

        // --- Business Logic Methods ---

        /// <summary>
        /// Updates the text of the comment.
        /// </summary>
        public void UpdateText(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText))
            {
                throw new ArgumentNullException(nameof(newText), "Comment text cannot be empty.");
            }

            Text = newText;
            AddDomainEvent(new CommentUpdatedEvent(this)); 
        }

        
    }
}
