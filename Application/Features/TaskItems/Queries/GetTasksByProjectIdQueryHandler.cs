using Application.Common.Interfaces; // Asume tu interfaz de DbContext
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System; // Necesario para Guid.Parse
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
// Asegúrate de que TaskItemDto esté en un namespace accesible, podría ser Application.Features.TaskItems.Queries;

namespace Application.Features.TaskItems.Queries.GetTasksByProjectId;

public class GetTasksByProjectIdQueryHandler : IRequestHandler<GetTasksByProjectIdQuery, List<TaskItemDto>>
{
    private readonly IApplicationDbContext _context; // Tu DbContext o repositorio
    private readonly IMapper _mapper;

    public GetTasksByProjectIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<TaskItemDto>> Handle(GetTasksByProjectIdQuery request, CancellationToken cancellationToken)
    {
        // === Conversión crucial: Parsear el ProjectId de string a Guid ===
        if (!Guid.TryParse(request.ProjectId, out Guid projectIdGuid))
        {
            throw new ArgumentException("El ID del proyecto proporcionado no es un formato GUID válido.");
        }

        var tasks = await _context.TaskItems
                                  // === Incluir propiedades de navegación para el mapeo ===
                                  .Include(t => t.Project)       // Necesario para ProjectName
                                  .Include(t => t.AssignedToUser) // Necesario para AssignedToUserName
                                                                  // === Aplicar el filtro por ProjectId (Guid) ===
                                  .Where(t => t.ProjectId == projectIdGuid)
                                  // === Ordenar por la propiedad correcta: Created ===
                                  .OrderByDescending(t => t.Created) // <--- ¡Cambiado de CreationDate a Created!
                                  .AsNoTracking() // Recomendado para consultas de solo lectura
                                  .ToListAsync(cancellationToken);

        // Mapear la lista de entidades TaskItem a una lista de TaskItemDto
        return _mapper.Map<List<TaskItemDto>>(tasks);
    }
}
