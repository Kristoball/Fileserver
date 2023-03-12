
using Application.Models;
using Domain.Models;
using Domain.Services;

namespace Application.Services;
public class ApplicationSecurity : IApplicationSecurity
{
    private readonly IHashingAlgorithm _hashingAlgorithm;
    private IList<IUser> _user = new List<IUser>() { 
        new User() { Active = true, Email="test", CustomerRoles = new List<IUserRole> { new AdminUser() }, Username = "kristofferE", Password = "test"} ,
        new User() { Active = true, Email="test2", CustomerRoles = new List<IUserRole> { new SubAdminUser() }, Username = "kristoffer", Password = "test"} ,
    };

    public ApplicationSecurity(IHashingAlgorithm hashingAlgorithm)
    {
        _hashingAlgorithm = hashingAlgorithm;
    }

    public async Task<IUser?> GetUser(string email)
    {
        var user = _user.SingleOrDefault(x => x.Email == email);
        return await Task.FromResult(user);
    }

    public async Task<IUser?> ValidateUser(string email, string password)
    {
        var result = await GetUser(email);
        if (result == null)
            return null;

        if (result.CheckPassword(_hashingAlgorithm, password))
            return result;

        return null;
    }
}
