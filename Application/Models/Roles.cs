using Domain.Models;

namespace Infrastructure.Models;
public class Admin: IApplicationRole
{
}
public class SubAdmin : IApplicationRole
{
}

public class AdminUser : IUserRole
{
    public IList<IApplicationRole> ApplicationRoles => new List<IApplicationRole>() { new Admin(), new SubAdmin() };
}
public class SubAdminUser : IUserRole
{
    public IList<IApplicationRole> ApplicationRoles => new List<IApplicationRole>() { new SubAdmin() };
}