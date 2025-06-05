using Application.Common.Mappings;
using Domain.Entities; // Asume que TaskItem es tu entidad de dominio
using AutoMapper; // Necesario para la configuración de mapeo detallada
using System;

namespace Application.Features.TaskItems.Queries;

public class TaskItemDto : IMapFrom<TaskItem>
{
    // Propiedades que se mapean a string para el frontend
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string DueDate { get; set; } = string.Empty; // Convertido a string "yyyy-MM-dd"
    public string Status { get; set; } = string.Empty; // Mapear el enum a string
    public string Priority { get; set; } = string.Empty; // Mapear el enum a string
    public string? CompletionDate { get; set; } // Convertido a string "yyyy-MM-dd" o null

    // Propiedades relacionadas, también mapeadas a string para el frontend
    public string ProjectId { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty; // Para mostrar el nombre del proyecto

    public string? AssignedToUserId { get; set; } // Convertido a string o null
    public string? AssignedToUserName { get; set; } // Para mostrar el nombre del usuario asignado

    // Propiedades de auditoría mapeadas desde BaseAuditableEntity
    public string CreationDate { get; set; } = string.Empty; // Mapeado desde src.Created
    public string? LastModifiedDate { get; set; } // Mapeado desde src.LastModified (nullable)


    // Implementación del método Mapping para configurar AutoMapper
    public void Mapping(Profile profile)
    {
        profile.CreateMap<TaskItem, TaskItemDto>()
            // Mapeo de Guid a string
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId.ToString()))
            .ForMember(dest => dest.AssignedToUserId, opt => opt.MapFrom(src => src.AssignedToUserId.HasValue ? src.AssignedToUserId.Value.ToString() : null))

            // Mapeo de Enums a string
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())) // Asume Status es un enum en tu entidad TaskItem
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString())) // Asume Priority es un enum en tu entidad TaskItem

            // Mapeo de DateTime a string (formatos para el frontend)
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate.ToString("yyyy-MM-dd"))) // Formato para input type="date" en frontend
            .ForMember(dest => dest.CompletionDate, opt => opt.MapFrom(src => src.CompletionDate.HasValue ? src.CompletionDate.Value.ToString("yyyy-MM-dd") : null))

            // --- ¡CORRECCIÓN AQUÍ! Mapeo de DateTimeOffset a string ---
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.Created.ToString("yyyy-MM-ddTHH:mm:ssZ"))) // Usar src.Created
            .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => src.LastModified.HasValue ? src.LastModified.Value.ToString("yyyy-MM-ddTHH:mm:ssZ") : null)) // Usar src.LastModified y manejar nullable

            // Mapeo de propiedades de navegación (requiere .Include en la consulta del QueryHandler)
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project != null ? src.Project.Name : "N/A"))
            .ForMember(dest => dest.AssignedToUserName, opt => opt.MapFrom(src => src.AssignedToUser != null ? src.AssignedToUser.Username : "N/A"));
    }
}
