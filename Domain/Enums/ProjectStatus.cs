
namespace Domain.Enums
{
    public enum ProjectStatus
    {
        /// <summary>
        /// The project is currently active and tasks are being worked on.
        /// </summary>
        Active = 0,

        /// <summary>
        /// The project has been completed successfully.
        /// </summary>
        Completed = 1,

        /// <summary>
        /// The project has been put on hold or indefinitely postponed.
        /// </summary>
        OnHold = 2,

        /// <summary>
        /// The project is no longer active and has been moved to an archive.
        /// </summary>
        Archived = 3
    }
}
