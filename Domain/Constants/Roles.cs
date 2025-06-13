namespace Domain.Constants
{
    public static class Roles
    {
        public const string Administrator = "Administrator"; 
        public const string ProjectManager = "ProjectManager"; 
        public const string TeamMember = "TeamMember";     
        public const string Viewer = "Viewer";           

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
