namespace Identity.Application.DTOs.Auth;

public class UserRoleDto
{
    public Guid UserId { get; set; }
    public List<Guid>? RoleId { get; set; }
}