using SharedKernel.Domain;
using static SharedKernel.Contracts.Enum;

namespace SharedKernel.Contracts;

public class TokenUser : EntityAuditBase
{
    public string Username { get; set; }

    public string PasswordHash { get; set; }

    public string Salt { get; set; }

    public string PhoneNumber { get; set; }

    public bool ConfirmedPhone { get; set; }

    public string Email { get; set; }

    public bool ConfirmedEmail { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Address { get; set; }

    public DateTime DateOfBirth { get; set; }

    public GenderType Gender { get; set; }
    
    public Guid PositionId { get; set; }

    public Guid DepartmentId { get; set; }
    
    public string Permission { get; set; }

    public List<string> RoleNames { get; set; } = new List<string>();
}