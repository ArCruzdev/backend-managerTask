using Domain.Common;
using Domain.Events;
using Domain.Constants;
using Domain.Exceptions;

namespace Domain.Entities
{
    public class User : BaseAuditableEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; } 
        public string Username { get; private set; } 
        public string Role { get; private set; } 
        public bool IsActive { get; private set; }

        
        private readonly List<TaskItem> _assignedTasks = new();
        public IReadOnlyCollection<TaskItem> AssignedTasks => _assignedTasks.AsReadOnly();

        private readonly List<Project> _memberOfProjects = new();
        public IReadOnlyCollection<Project> MemberOfProjects => _memberOfProjects.AsReadOnly();

        private readonly List<Comment> _comments = new();
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
        
        private User() { }

        // Public constructor for creating a new user
        public User(string firstName, string lastName, string email, string username, string role)
        {
            // Basic validations
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentNullException(nameof(firstName), "First name cannot be empty.");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentNullException(nameof(lastName), "Last name cannot be empty.");
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email)) 
                throw new ArgumentException("Invalid email format.", nameof(email));
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username), "Username cannot be empty.");
            if (string.IsNullOrWhiteSpace(role) || !Roles.GetAllRoles().Contains(role)) 
                throw new InvalidUserOperationException("Invalid or unsupported user role.");

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Username = username;
            Role = role;
            IsActive = true; 

            AddDomainEvent(new UserCreatedEvent(this)); 
        }

        // --- Business Logic Methods ---

        /// <summary>
        /// Updates the user's basic profile information.
        /// </summary>
        public void UpdateProfile(string newFirstName, string newLastName, string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newFirstName))
                throw new ArgumentNullException(nameof(newFirstName), "First name cannot be empty.");
            if (string.IsNullOrWhiteSpace(newLastName))
                throw new ArgumentNullException(nameof(newLastName), "Last name cannot be empty.");
            if (string.IsNullOrWhiteSpace(newEmail) || !IsValidEmail(newEmail))
                throw new ArgumentException("Invalid email format.", nameof(newEmail));

            FirstName = newFirstName;
            LastName = newLastName;
            Email = newEmail;

            AddDomainEvent(new UserProfileUpdatedEvent(this)); 
        }

        /// <summary>
        /// Changes the user's role.
        /// </summary>
        public void ChangeRole(string newRole)
        {
            if (string.IsNullOrWhiteSpace(newRole) || !Roles.GetAllRoles().Contains(newRole))
            {
                throw new InvalidUserOperationException("Invalid or unsupported user role.");
            }

            if (Role == newRole) return; 

            string oldRole = Role;
            Role = newRole;
            AddDomainEvent(new UserRoleChangedEvent(this, oldRole, newRole)); 
        }

        /// <summary>
        /// Deactivates the user account.
        /// A deactivated user cannot be assigned to new tasks or projects.
        /// Any currently assigned tasks or projects should be handled (e.g., unassigned).
        /// </summary>
        public void Deactivate()
        {
            if (!IsActive) return; 

            IsActive = false;

            
            foreach (var task in _assignedTasks.Where((Func<TaskItem, bool>)(t => t.Status == Enums.TaskItemStatus.Pending || t.Status == Enums.TaskItemStatus.InProgress)))
            {
                task.UnassignUser(); 
            }

            AddDomainEvent(new UserDeactivatedEvent(this));
        }

        /// <summary>
        /// Activates the user account.
        /// </summary>
        public void Activate()
        {
            if (IsActive) return; 

            IsActive = true;
            AddDomainEvent(new UserActivatedEvent(this)); 
        }

        // Helper method for email validation (can be more robust with regex)
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        
        internal void AddToProject(Project project)
        {
            if (!_memberOfProjects.Any(p => p.Id == project.Id))
            {
                _memberOfProjects.Add(project);
            }
        }

        internal void RemoveFromProject(Project project)
        {
            _memberOfProjects.RemoveAll(p => p.Id == project.Id);
        }

        public void AssignTask(TaskItem task)
        {
            if (!IsActive)
                throw new InvalidUserOperationException("Cannot assign tasks to an inactive user.");

            if (!_assignedTasks.Any(t => t.Id == task.Id))
            {
                _assignedTasks.Add(task);
            }
        }

        public void RemoveTask(TaskItem task)
        {
            _assignedTasks.RemoveAll(t => t.Id == task.Id);
        }

    }
}
