// Path: Application/Features/Users/Queries/UserDto.cs
using Application.Common.Mappings;
using Domain.Entities; // Necesitamos acceso a la entidad User

namespace Application.Features.Users.Queries;

public class UserDto : IMapFrom<User>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // Mapear el rol a string
    public bool IsActive { get; set; }
}
