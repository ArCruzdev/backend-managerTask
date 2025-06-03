using Domain.Common;
using Domain.Entities;

namespace Domain.Events
{
    /// <summary>
    /// Event triggered when a new User is created.
    /// </summary>
    public class UserCreatedEvent : BaseEvent
    {
        public User User { get; }

        public UserCreatedEvent(User user)
        {
            User = user;
        }
    }

    /// <summary>
    /// Event triggered when a User's profile details are updated.
    /// </summary>
    public class UserProfileUpdatedEvent : BaseEvent
    {
        public User User { get; }

        public UserProfileUpdatedEvent(User user)
        {
            User = user;
        }
    }

    /// <summary>
    /// Event triggered when a User's role is changed.
    /// </summary>
    public class UserRoleChangedEvent : BaseEvent
    {
        public User User { get; }
        public string OldRole { get; }
        public string NewRole { get; }

        public UserRoleChangedEvent(User user, string oldRole, string newRole)
        {
            User = user;
            OldRole = oldRole;
            NewRole = newRole;
        }
    }

    /// <summary>
    /// Event triggered when a User account is deactivated.
    /// </summary>
    public class UserDeactivatedEvent : BaseEvent
    {
        public User User { get; }

        public UserDeactivatedEvent(User user)
        {
            User = user;
        }
    }

    /// <summary>
    /// Event triggered when a User account is activated.
    /// </summary>
    public class UserActivatedEvent : BaseEvent
    {
        public User User { get; }

        public UserActivatedEvent(User user)
        {
            User = user;
        }
    }
}
