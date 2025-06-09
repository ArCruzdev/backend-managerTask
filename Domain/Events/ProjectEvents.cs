using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    /// <summary>
    /// Event triggered when a new Project is created.
    /// </summary>
    public class ProjectCreatedEvent : BaseEvent
    {
        public Project Project { get; }

        public ProjectCreatedEvent(Project project)
        {
            Project = project;
        }
    }

    /// <summary>
    /// Event triggered when a Project's details are updated.
    /// </summary>
    public class ProjectUpdatedEvent : BaseEvent
    {
        public Project Project { get; }

        public ProjectUpdatedEvent(Project project)
        {
            Project = project;
        }
    }

    /// <summary>
    /// Event triggered when a Project's status changes.
    /// </summary>
    public class ProjectStatusChangedEvent : BaseEvent
    {
        public Project Project { get; }
        public ProjectStatus NewStatus { get; }

        public ProjectStatusChangedEvent(Project project, ProjectStatus newStatus)
        {
            Project = project;
            NewStatus = newStatus;
        }
    }

    /// <summary>
    /// Event triggered when a User is added as a member to a Project.
    /// </summary>
    public class ProjectMemberAddedEvent : BaseEvent
    {
        public Project Project { get; }
        public User Member { get; }

        public ProjectMemberAddedEvent(Project project, User member)
        {
            Project = project;
            Member = member;
        }
    }

    /// <summary>
    /// Event triggered when a User is removed as a member from a Project.
    /// </summary>
    public class ProjectMemberRemovedEvent : BaseEvent
    {
        public Project Project { get; }
        public User Member { get; }

        public ProjectMemberRemovedEvent(Project project, User member)
        {
            Project = project;
            Member = member;
        }
    }

    /// <summary>
    /// Event triggered when a Project is removed.
    /// </summary>
    public class ProjectDeletedEvent : BaseEvent
    {
        public Project Project { get; }

        public ProjectDeletedEvent(Project project)
        {
            Project = project;
        }
    }
}
