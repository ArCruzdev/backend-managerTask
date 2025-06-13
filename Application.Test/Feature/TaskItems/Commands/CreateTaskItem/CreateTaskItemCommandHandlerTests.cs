using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Features.TaskItems.Commands.CreateTaskItem;
using Domain.Entities;
using Moq;
using Xunit;
using Application.Tests.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Tests.Features.TaskItems.Commands.CreateTaskItem
{
    public class CreateTaskItemCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;
        private readonly CreateTaskItemCommandHandler _handler;

        private readonly List<Project> _projectList;
        private readonly List<User> _userList;
        private readonly List<TaskItem> _taskItemList;

        public CreateTaskItemCommandHandlerTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();

            _projectList = new List<Project>
            {
                new Project { Id = Guid.NewGuid(), Name = "Test Project" }
            };

            _userList = new List<User>
            {
                new User { Id = Guid.NewGuid(), Name = "Test User" }
            };

            _taskItemList = new List<TaskItem>();

            var mockProjects = DbSetMock.GetQueryableMockDbSet(_projectList);
            var mockUsers = DbSetMock.GetQueryableMockDbSet(_userList);
            var mockTaskItems = DbSetMock.GetQueryableMockDbSet(_taskItemList);

            _mockContext.Setup(x => x.Projects).Returns(mockProjects.Object);
            _mockContext.Setup(x => x.Users).Returns(mockUsers.Object);
            _mockContext.Setup(x => x.TaskItems).Returns(mockTaskItems.Object);
            _mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _handler = new CreateTaskItemCommandHandler(_mockContext.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateTask_WhenValidData()
        {
            // Arrange
            var projectId = _projectList[0].Id;
            var userId = _userList[0].Id;

            var command = new CreateTaskItemCommand
            {
                Title = "Test Task",
                Description = "Test description",
                DueDate = DateTime.Today.AddDays(1),
                ProjectId = projectId,
                AssignedToUserId = userId
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            Assert.Single(_taskItemList);
        }
    }
}
