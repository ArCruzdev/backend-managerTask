using Domain.Common;
using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;

namespace Domain.Entities
{
    public class Project : BaseAuditableEntity
    {
        public string Name { get; private set; } 
        public string? Description { get; private set; } 
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; } 
        public ProjectStatus Status { get; private set; }
        public decimal? Budget { get; private set; } 
        
        private readonly List<TaskItem> _tasks = new();
        public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();

        
        private readonly List<User> _members = new();
        public IReadOnlyCollection<User> Members => _members.AsReadOnly();

        
        private Project()
        {
            
        }

        // Public constructor for creating new projects
        public Project(string name, DateTime startDate, string? description = null, decimal? budget = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Project name cannot be empty.");
            }
            if (startDate == default) 
            {
                throw new ArgumentException("Start date must be a valid date.", nameof(startDate));
            }

            Name = name;
            Description = description;
            StartDate = startDate;
            Status = ProjectStatus.Active; 
            Budget = budget;

            
            AddDomainEvent(new ProjectCreatedEvent(this)); 
        }

        // --- Business Logic Methods ---

        /// <summary>
        /// Updates the project's details.
        /// </summary>
        public void UpdateDetails(string name, string? description, DateTime? endDate, decimal? budget)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Project name cannot be empty.");
            }
            if (endDate.HasValue && endDate.Value < StartDate)
            {
                throw new InvalidProjectOperationException("End date cannot be before start date.");
            }

            Name = name;
            Description = description;
            EndDate = endDate;
            Budget = budget;
            AddDomainEvent(new ProjectUpdatedEvent(this)); 
        }

        /// <summary>
        /// Changes the status of the project.
        /// </summary>
        public void ChangeStatus(ProjectStatus newStatus)
        {
            if (Status == newStatus)
            {
                return; 
            }

           
            if (newStatus == ProjectStatus.Completed && _tasks.Any(t => t.Status == TaskItemStatus.Pending || t.Status == TaskItemStatus.InProgress))
            {
                throw new InvalidProjectOperationException("Cannot complete project with pending or in-progress tasks.");
            }

            
            if (newStatus == ProjectStatus.Archived)
            {
                foreach (var task in _tasks.Where(t => t.Status == TaskItemStatus.Pending || t.Status == TaskItemStatus.InProgress))
                {
                    task.Cancel(); 
                }
            }

            Status = newStatus;
            AddDomainEvent(new ProjectStatusChangedEvent(this, newStatus)); 
        }

        /// <summary>
        /// Adds a user as a member to the project.
        /// </summary>
        public void AddMember(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (_members.Any(m => m.Id == user.Id))
            {
                throw new InvalidProjectOperationException($"User '{user.Email}' is already a member of this project.");
            }

            _members.Add(user);
            AddDomainEvent(new ProjectMemberAddedEvent(this, user)); 
        }

        /// <summary>
        /// Removes a user from the project members.
        /// If the user is assigned to any tasks in this project, those assignments should be handled (e.g., unassigned, reassigned, or exception).
        /// For simplicity, let's just unassign them from any tasks they own in this project.
        /// </summary>
        public void RemoveMember(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (!_members.Any(m => m.Id == user.Id))
            {
                throw new InvalidProjectOperationException($"User '{user.Email}' is not a member of this project.");
            }

            
            foreach (var task in _tasks.Where(t => t.AssignedToUserId == user.Id && (t.Status == TaskItemStatus.Pending || t.Status == TaskItemStatus.InProgress)))
            {
                task.UnassignUser(); 
            }

            _members.Remove(_members.First(m => m.Id == user.Id));
            AddDomainEvent(new ProjectMemberRemovedEvent(this, user)); 
        }

        public void FinalizeIfAllTasksCompleted()
        {
            if (Status == ProjectStatus.Completed || Status == ProjectStatus.Archived)
                return; 

            if (_tasks.Any() && _tasks.All(t => t.Status == TaskItemStatus.Completed))
            {
                Status = ProjectStatus.Completed;
                EndDate = DateTime.UtcNow;

                AddDomainEvent(new ProjectStatusChangedEvent(this, ProjectStatus.Completed));
            }
        }
        //validacion si se puede o no eliminar el project
        public void EnsureCanBeDeleted()
        {
            if (Status == ProjectStatus.Completed)
            {
                throw new InvalidProjectOperationException("Cannot delete a completed project.");
            }
        }



    }
}
