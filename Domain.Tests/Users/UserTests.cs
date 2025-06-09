using Domain.Constants;
using Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace Domain.Tests.Users
{
    public class UserTests
    {
        [Fact]
        public void Deactivate_ShouldSetIsActiveToFalse_AndUnassignActiveTasks()
        {
            // Arrange
            var user = new User("Ana", "Pérez", "ana@example.com", "ana123", Roles.Administrator);


            var task1 = new TaskItem("Tarea 1", DateTime.UtcNow.AddDays(1), Guid.NewGuid());
            var task2 = new TaskItem("Tarea 2", DateTime.UtcNow.AddDays(1), Guid.NewGuid());

            task1.AssignToUser(user);
            task2.AssignToUser(user);

            // Simula tareas asignadas
            user.GetType().GetField("_assignedTasks", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(user, new List<TaskItem> { task1, task2 });

            // Act
            user.Deactivate();

            // Assert
            user.IsActive.Should().BeFalse();
            task1.AssignedToUserId.Should().BeNull();
            task2.AssignedToUserId.Should().BeNull();
        }
    }
}

