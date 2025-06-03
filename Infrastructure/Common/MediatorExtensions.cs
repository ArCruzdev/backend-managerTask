using MediatR;
using Domain.Common; // Para BaseEntity y BaseEvent
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Common; 

public static class MediatorExtensions
{
    public static async Task DispatchDomainEvents(this IMediator mediator, DbContext context)
    {
        // Obtener todas las entidades que tienen eventos de dominio sin publicar
        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        // Limpiar los eventos de las entidades para evitar que se publiquen dos veces
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            // Publicar cada evento de dominio usando MediatR
            await mediator.Publish(domainEvent);
        }
    }
}
