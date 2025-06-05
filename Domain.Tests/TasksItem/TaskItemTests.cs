using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Domain.Tests.TaskItems;

public class TaskItemTests
{
    [Fact]
    public void Complete_Should_SetStatusToCompleted_AndSetCompletionDate()
    {
        // Arrange
        var task = new TaskItem("Prueba", DateTime.UtcNow.AddDays(1), Guid.NewGuid());

        // Act
        task.Complete();

        // Assert
        task.Status.Should().Be(TaskItemStatus.Completed);
        task.CompletionDate.Should().NotBeNull();
    }

    [Fact]
    public void Cancel_Should_SetStatusToCanceled()
    {
        // Arrange
        var task = new TaskItem("Cancelar tarea", DateTime.UtcNow.AddDays(1), Guid.NewGuid());

        // Act
        task.Cancel();

        // Assert
        task.Status.Should().Be(TaskItemStatus.Canceled);
    }

    [Fact]
    public void StartProgress_Should_SetStatusToInProgress()
    {
        // Arrange
        var task = new TaskItem("Iniciar tarea", DateTime.UtcNow.AddDays(1), Guid.NewGuid());

        // Act
        task.StartProgress();

        // Assert
        task.Status.Should().Be(TaskItemStatus.InProgress);
    }

    [Fact]
    public void AssignToUser_Should_SetUserFields()
    {
        // Arrange
        var user = new User("Luis", "Gomez", "luis@test.com", "luis123", "Administrator");
        var task = new TaskItem("Asignar tarea", DateTime.UtcNow.AddDays(1), Guid.NewGuid());

        // Act
        task.AssignToUser(user);

        // Assert
        task.AssignedToUserId.Should().Be(user.Id);
        task.AssignedToUser.Should().Be(user);
    }

    [Fact]
    public void AssignToUser_Should_ThrowException_IfUserIsInactive()
    {
        // Arrange
        var user = new User("Carlos", "Inactivo", "carlos@x.com", "carlosx", "ProjectManager");
        user.Deactivate(); // Set user as inactive
        var task = new TaskItem("Asignar tarea", DateTime.UtcNow.AddDays(1), Guid.NewGuid());

        // Act
        Action act = () => task.AssignToUser(user);

        // Assert
        act.Should().Throw<InvalidTaskOperationException>();
    }

    [Fact]
    public void UnassignUser_Should_ClearAssignment()
    {
        // Arrange
        var user = new User("Ana", "Activa", "ana@x.com", "anauser", "TeamMember");
        var task = new TaskItem("Desasignar", DateTime.UtcNow.AddDays(1), Guid.NewGuid());
        task.AssignToUser(user);

        // Act
        task.UnassignUser();

        // Assert
        task.AssignedToUser.Should().BeNull();
        task.AssignedToUserId.Should().BeNull();
    }

    [Fact]
    public void UpdateDetails_Should_Update_FieldsCorrectly()
    {
        // Arrange
        var task = new TaskItem("Original", DateTime.UtcNow.AddDays(1), Guid.NewGuid());
        var newTitle = "Actualizada";
        var newDesc = "Nueva descripción";
        var newDate = DateTime.UtcNow.AddDays(5);
        var newPriority = TaskPriority.High;

        // Act
        task.UpdateDetails(newTitle, newDesc, newDate, newPriority);

        // Assert
        task.Title.Should().Be(newTitle);
        task.Description.Should().Be(newDesc);
        task.DueDate.Should().Be(newDate);
        task.Priority.Should().Be(TaskPriority.High);
    }

    [Fact]
    public void EnsureCanBeDeleted_Should_ThrowException_IfStatusIsCompleted()
    {
        // Arrange
        var task = new TaskItem("Completar y borrar", DateTime.UtcNow.AddDays(1), Guid.NewGuid());
        task.Complete();

        // Act
        Action act = () => task.EnsureCanBeDeleted();

        // Assert
        act.Should().Throw<InvalidTaskOperationException>();
    }

}


