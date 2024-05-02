using Identity.Domain.Entities;
using SharedKernel.Application;
using SharedKernel.Libraries;

namespace Identity.Infrastructure.Persistence.Data;

public static class Default
{
    private static Guid id = Guid.NewGuid();
    
    public static Guid GetSupperAdminId
    {
        get => id;
    }
    public static User GetSupperAdmin()
    {
        return new User()
        {
            Id = id,
            Username = "supperadmin",
            PasswordHash = "supperadmin".ToMD5(),
            Salt = Utility.RandomString(6),
            PhoneNumber = "0976580418",
            ConfirmedPhone = true,
            Email = "devbe2002@gmail.com",
            ConfirmedEmail = true,
            FirstName = "Đỗ Chí",
            LastName = "Hùng",
            Address = "Đông Kết, Khoái Châu, Hưng Yên",
            DateOfBirth = new DateTime(2002, 9, 6).ToUniversalTime(),
            Gender = GenderType.Other,
            CreatedDate = DateHelper.Now,
            CreatedBy = id,
            LastModifiedDate = null,
            LastModifiedBy = null,
            DeletedDate = null,
            DeletedBy = null,
            IsDeleted = false
        };
    }
    
    public static User GetAdmin()
    {
        return new User()
        {
            Id = Guid.NewGuid(),
            Username = "admin",
            PasswordHash = "admin".ToMD5(),
            Salt = Utility.RandomString(6),
            PhoneNumber = "0398707310",
            ConfirmedPhone = true,
            Email = "dochihung492002@gmail.com",
            ConfirmedEmail = true,
            FirstName = "Đỗ Chí",
            LastName = "Hòa",
            Address = "Đông Kết, Khoái Châu, Hưng Yên",
            DateOfBirth = new DateTime(2002, 09, 06).ToUniversalTime(),
            Gender = GenderType.Other,
            CreatedDate = DateHelper.Now,
            CreatedBy = id,
            LastModifiedDate = null,
            LastModifiedBy = null,
            DeletedDate = null,
            DeletedBy = null,
            IsDeleted = false
        };
    }
    
    public static IEnumerable<Role> GetRoles()
    {
        return new List<Role>()
        {
            new Role
            {
                Code = RoleConstant.SupperAdmin, Name = "Super Admin", IsDeleted = false, CreatedDate = DateHelper.Now,
                CreatedBy = id, LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null
            },
            new Role
            {
                Code = RoleConstant.Admin, Name = "Admin", IsDeleted = false, CreatedDate = DateHelper.Now,
                CreatedBy = id, LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null
            },
            new Role
            {
                Code = RoleConstant.Teacher, Name = "Teacher", IsDeleted = false, CreatedDate = DateHelper.Now,
                CreatedBy = id, LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null
            },
            new Role
            {
                Code = RoleConstant.Student, Name = "Student", IsDeleted = false, CreatedDate = DateHelper.Now,
                CreatedBy = id, LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null
            },
            new Role
            {
                Code = RoleConstant.User, Name = "User", IsDeleted = false, CreatedDate = DateHelper.Now,
                CreatedBy = id, LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null
            }
        };
    }

    public static IEnumerable<Permission> GetPermissions()
    {
        var permissions = new List<Permission>();
        foreach (ActionExponent action in Enum.GetValues(typeof(ActionExponent)))
        {
            var code = action.ToString().ToSnakeCaseUpper();
            var name = action.ToString().PascalToStandard();
            permissions.Add(new Permission { Code = code, Name = name, Exponent = (int)action, IsDeleted = false, CreatedDate = DateHelper.Now, CreatedBy = id, LastModifiedDate = null, LastModifiedBy = null, DeletedDate = null, DeletedBy = null });
        }

        return permissions;
    }
}