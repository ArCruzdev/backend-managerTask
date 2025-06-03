
namespace Domain.Constants
{
    public static class Roles
    {
        public const string Administrator = "Administrator"; // Full system access, project management, user management.
        public const string ProjectManager = "ProjectManager"; // Manage projects, assign tasks, view progress.
        public const string TeamMember = "TeamMember";     // Work on assigned tasks, add comments.
        public const string Viewer = "Viewer";           // Read-only access to tasks and projects.

        /// <summary>
        /// Gets all defined user roles as a read-only collection.
        /// </summary>
        public static IReadOnlyCollection<string> GetAllRoles()
        {
         
            return typeof(Roles)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(fi => (string)fi.GetRawConstantValue()!)
                .ToList()
                .AsReadOnly();
        }
    }
}
