using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models;
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