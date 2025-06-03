
namespace Domain.Enums
{
    public enum TaskItemStatus
    {
        /// <summary>
        /// The task has been created but no work has started yet.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// The task is currently being worked on.
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// The task has been completed successfully.
        /// </summary>
        Completed = 2,

        /// <summary>
        /// The task has been cancelled and will not be completed.
        /// </summary>
        Canceled = 3
    }
}
