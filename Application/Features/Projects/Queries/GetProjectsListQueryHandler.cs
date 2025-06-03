using AutoMapper;
using MediatR;
using Application.Common.Interfaces; 
using Domain.Entities; 
using Microsoft.EntityFrameworkCore; 

namespace Application.Features.Projects.Queries;

// IRequestHandler<TRequest, TResponse> es la interfaz de MediatR para manejar queries/comandos.
public class GetProjectsListQueryHandler : IRequestHandler<GetProjectsListQuery, List<ProjectDto>>
{
    private readonly IApplicationDbContext _context; // Interfaz de la base de datos (repositorio)
    private readonly IMapper _mapper; // Interfaz de AutoMapper

    // Inyección de dependencias para el contexto de la base de datos y AutoMapper
    public GetProjectsListQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Método principal para manejar la query
    public async Task<List<ProjectDto>> Handle(GetProjectsListQuery request, CancellationToken cancellationToken)
    {
        // 1. Obtener los datos del dominio (a través del repositorio/DbContex)
        // Usamos AsNoTracking() para queries de lectura, ya que no modificaremos las entidades.
        var projects = await _context.Projects
                                     .AsNoTracking() // Optimización para queries de lectura
                                     .OrderBy(p => p.Name) // Ordenamos por nombre
                                     .ToListAsync(cancellationToken);

        // 2. Mapear las entidades de dominio a DTOs
        // AutoMapper se encargará de transformar List<Project> a List<ProjectDto>
        var projectDtos = _mapper.Map<List<ProjectDto>>(projects);

        return projectDtos;
    }
}
