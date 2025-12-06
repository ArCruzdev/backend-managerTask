using Application.Features.Projects.Queries;
using Application.Common.Interfaces;
using AutoMapper;
using Moq;
using Xunit;
using Application.Test.TestData.ProjectTestData;


namespace Application.Test.Feature.ProjectsItems.Query;

public class GetProjectByIdQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly Mock<IMapper> _mapper;
    private readonly GetProjectByIdQueryHandler _handler;

    public GetProjectByIdQueryHandlerTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _mapper = new Mock<IMapper>(); 
        _handler = new GetProjectByIdQueryHandler(_mockContext.Object, _mapper.Object);
    }

    [Fact(DisplayName = "Handler - Devuelve un projectDto cuanto el proyecto existe")]
    public async Task Handle_shouel_return_projectDto_when_project_exist()
    {
        // Arrange
        var project = ProjectTestData.GetPoject();
        var projectDto = ProjectTestData.GetPojectDto();
        var query = new GetProjectByIdQuery(project.Id);
        _mockContext
            .Setup(r => r.Projects.FindAsync(project.Id))
            .ReturnsAsync(project);
        _mapper
            .Setup(x => x.Map<ProjectDto>(project))
            .Returns(projectDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(projectDto, result);

    }
}

