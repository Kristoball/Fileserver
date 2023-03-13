using Domain.Services;

namespace Domain.Models;
public interface IUser
{
    public Guid Id { get; set; }
    IDictionary<string, string> Preferences { get; set; }
	string Email { get; set; }
	IEnumerable<IApplicationRole> ApplicationRoles { get; }
	IEnumerable<IUserRole> CustomerRoles { get; set; }
	string Username { get; set; }
	bool Active { get; set; }
	bool CheckPassword(IHashingAlgorithm algorithm, string password);

}