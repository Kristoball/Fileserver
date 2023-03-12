using Domain.Models;
using Domain.Services;

namespace Application.Models;

public class User : IUser
{
    public IDictionary<string, string> Preferences { get; set; }
    public string Email { get; set; }

    public IEnumerable<IApplicationRole> ApplicationRoles => CustomerRoles.SelectMany(x=>x.ApplicationRoles);

    public IEnumerable<IUserRole> CustomerRoles { get; set; }
    public string Username { get; set; }
    public bool Active { get; set; }
    public string Password { get; set; }
    public bool CheckPassword(IHashingAlgorithm algorithm, string password)
    {
        return Password == password;
    }
}
