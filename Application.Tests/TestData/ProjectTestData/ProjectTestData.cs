using System.Xml.Linq;
using Application.Features.Projects.Queries;
using Domain.Entities;
using Domain.Enums;

namespace Application.Test.TestData.ProjectTestData;

public static class ProjectTestData
{
    public static Project GetPoject()
    {
        var id = Guid.NewGuid();
        return new Project(id, "proyecto prueba", DateTime.UtcNow);
    }

    public static ProjectDto GetPojectDto() 
    {
        return new ProjectDto()
        {
            Id = Guid.NewGuid(),
            Name = "Proyecto prueba",
            Description = "proyecto para pruebas unitarias",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow,
            Status = "actibvo",
            HasActiveTasks = true
        };
    }

}

