
using Application.Models;
using Domain.Models;
using Domain.Services;
using System.Security.Claims;
using System.Security.Principal;

namespace Application.Services;
public class ApplicationSecurity : IApplicationSecurity
{
    private readonly IHashingAlgorithm _hashingAlgorithm;
    private IList<IUser> _user = new List<IUser>() { 
        new User() { Active = true, Email="test", CustomerRoles = new List<IUserRole> { new AdminUser() }, Username = "kris", Password = "test"} ,
        new User() { Active = true, Email="test2", CustomerRoles = new List<IUserRole> { new SubAdminUser() }, Username = "kristoffer", Password = "test"} ,
    };
    private IList<LoginToken> _loginTokens = new List<LoginToken>();

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

    public async Task<Guid> CreateLoginToken(IPrincipal principal)
    {
        if (string.IsNullOrEmpty(principal?.Identity?.Name))
            return Guid.Empty;

        var identity = (ClaimsPrincipal)principal;

        var emailClaim = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
        if (emailClaim == null)
            return await Task.FromResult(Guid.Empty);

        var logintoken = new LoginToken()
        {
            Email = emailClaim.Value,
            Created = DateTime.UtcNow,
            Token = Guid.NewGuid(),
        };

        _loginTokens.Add(logintoken);
        return await Task.FromResult(logintoken.Token);
    }

    public async Task<IUser?> ValidateUser(Guid token)
    {
        var email = _loginTokens.FirstOrDefault(x => x.Token == token && x.Created > DateTime.UtcNow.AddHours(-2))?.Email;

        if (string.IsNullOrEmpty(email))
            return null;

        return await GetUser(email);
    }

    private class LoginToken
    {
        public string Email { get; set; }
        public Guid Token { get; set; }
        public DateTime Created { get; set; }
    }
}
