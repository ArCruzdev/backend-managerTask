
namespace Domain.Enums
{
    public enum TaskPriority
    {
        /// <summary>
        /// Lowest priority, can be handled when other tasks are done.
        /// </summary>
        Low = 0,

        /// <summary>
        /// Normal priority, to be handled in due course.
        /// </summary>
        Medium = 1,

        /// <summary>
        /// Higher priority, should be addressed before medium or low tasks.
        /// </summary>
        High = 2,

        /// <summary>
        /// Highest priority, requires immediate attention.
        /// </summary>
        Critical = 3
    }
}
