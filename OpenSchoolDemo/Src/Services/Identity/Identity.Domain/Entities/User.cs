
namespace Identity.Domain.Entities;

[Table(TableName.User)]
public class User : EntityAuditBase
{
    public string Username { get; set; }

    public string PasswordHash { get; set; }

    public string Salt { get; set; }

    public string? PhoneNumber { get; set; }

    public bool? ConfirmedPhone { get; set; }

    public string? Email { get; set; }

    public bool? ConfirmedEmail { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? Address { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public GenderType? Gender { get; set; }
    
    #region [REFRENCE PROPERTIES]
    
    public virtual Avatar Avatar { get; set; }
    public virtual UserConfig UserConfig { get; set; }
    public virtual RefreshToken RefreshToken { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<SignInHistory> SignInHistories { get; set; }

    #endregion
}