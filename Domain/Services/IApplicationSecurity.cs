using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services;
public interface IApplicationSecurity
{
	Task<IUser?> GetUser(string email);
	Task<IUser?> ValidateUser(string username, string password);
	Task<IUser?> ValidateUser(Guid token);
	Task<Guid> CreateLoginToken(IPrincipal principal);
}
