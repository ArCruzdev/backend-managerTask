using MediatR;
using System.Collections.Generic;

namespace Application.Features.Projects.Queries;

// Una query simple que no necesita parámetros.
// IRequest<T> indica que esta Query devolverá una lista de ProjectDto.
public record GetProjectsListQuery : IRequest<List<ProjectDto>>
{
    // No necesitamos propiedades para esta query simple
}