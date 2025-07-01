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
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

            var testProjectId = Guid.NewGuid();
            _projectList = new List<Project>
            {
                new Project(testProjectId, "Test Project", DateTime.Today)
            };

            var testUserId = Guid.NewGuid();
            _userList = new List<User>
            {
                new User("Test", "User", "test.user@example.com", "testuser", "Administrator")
            };

            _taskItemList = new List<TaskItem>();

            var mockProjects = DbSetMock.GetQueryableMockDbSet(_projectList);
            var mockUsers = DbSetMock.GetQueryableMockDbSet(_userList);

            // --- INICIO DE LA CORRECCIÓN FINAL ---
            // Configuramos el mock de DbSet de TaskItem con un Callback
            // para que los elementos se añadan a nuestra lista en memoria.
            var mockTaskItems = new Mock<DbSet<TaskItem>>();

            mockTaskItems.Setup(m => m.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
                         // El Callback añade el objeto a la lista de prueba.
                         .Callback<TaskItem, CancellationToken>((entity, token) => _taskItemList.Add(entity))
                         // La CORRECCIÓN: devolvemos un ValueTask vacío para evitar el error de constructor.
                         .Returns(new ValueTask<EntityEntry<TaskItem>>());

            // Configuramos el DbSet mock para que sea "queryable".
            mockTaskItems.As<IQueryable<TaskItem>>().Setup(m => m.Provider).Returns(_taskItemList.AsQueryable().Provider);
            mockTaskItems.As<IQueryable<TaskItem>>().Setup(m => m.Expression).Returns(_taskItemList.AsQueryable().Expression);
            mockTaskItems.As<IQueryable<TaskItem>>().Setup(m => m.ElementType).Returns(_taskItemList.AsQueryable().ElementType);
            mockTaskItems.As<IQueryable<TaskItem>>().Setup(m => m.GetEnumerator()).Returns(() => _taskItemList.GetEnumerator());
            // --- FIN DE LA CORRECCIÓN FINAL ---

            // Configuraciones de FindAsync para Project y User.
            mockProjects.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                        .Returns<object[], CancellationToken>((ids, token) => new ValueTask<Project>(_projectList.FirstOrDefault(p => p.Id == (Guid)ids[0])));

            mockUsers.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                     .Returns<object[], CancellationToken>((ids, token) => new ValueTask<User>(_userList.FirstOrDefault(u => u.Id == (Guid)ids[0])));

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

            _mockContext.Verify(c => c.TaskItems.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
