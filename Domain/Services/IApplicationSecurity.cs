using Domain.Models;

namespace Domain.Services;
public interface IApplicationSecurity
{
	Task<IUser?> GetUser(string email);
	Task<IUser?> ValidateUser(string username, string password);
}
