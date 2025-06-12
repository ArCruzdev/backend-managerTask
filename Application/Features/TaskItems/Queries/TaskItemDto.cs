using Application.Common.Mappings;
using Domain.Entities; 
using AutoMapper; 
using System;

namespace Application.Features.TaskItems.Queries;

public class TaskItemDto : IMapFrom<TaskItem>
{
    
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string DueDate { get; set; } = string.Empty; 
    public string Status { get; set; } = string.Empty; 
    public string Priority { get; set; } = string.Empty; 
    public string? CompletionDate { get; set; } 
    public string ProjectId { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty; 
    public string? AssignedToUserId { get; set; } 
    public string? AssignedToUserName { get; set; } 
    public string CreationDate { get; set; } = string.Empty; 
    public string? LastModifiedDate { get; set; } 

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TaskItem, TaskItemDto>()
            
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId.ToString()))
            .ForMember(dest => dest.AssignedToUserId, opt => opt.MapFrom(src => src.AssignedToUserId.HasValue ? src.AssignedToUserId.Value.ToString() : null))

            
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())) 
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString())) 

            
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate.ToString("yyyy-MM-dd"))) 
            .ForMember(dest => dest.CompletionDate, opt => opt.MapFrom(src => src.CompletionDate.HasValue ? src.CompletionDate.Value.ToString("yyyy-MM-dd") : null))

            
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.Created.ToString("yyyy-MM-ddTHH:mm:ssZ"))) 
            .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => src.LastModified.HasValue ? src.LastModified.Value.ToString("yyyy-MM-ddTHH:mm:ssZ") : null)) // Usar src.LastModified y manejar nullable

            
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project != null ? src.Project.Name : "N/A"))
            .ForMember(dest => dest.AssignedToUserName, opt => opt.MapFrom(src => src.AssignedToUser != null ? src.AssignedToUser.Username : "N/A"));
    }
}
