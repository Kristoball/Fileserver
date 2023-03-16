using Domain.Models;

namespace Domain.Services;
public interface IApplicationSecurity
{
	Task<IUser?> GetUser(string email);
	Task<IUser?> ValidateUser(string username, string password);
	Task RegisterUser(string userName, string name, string password);
}
