using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Services;
public class AuthenticationStateProvider : IAuthenticationStateProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IApplicationSecurity _security;

    public AuthenticationStateProvider(IHttpContextAccessor contextAccessor, IApplicationSecurity security)
    {
        _contextAccessor = contextAccessor;
        _security = security;
    }

    private ClaimsPrincipal GetClaimsPrincipal(IUser user)
    {
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Email, user.Email));
        claims.Add(new Claim(ClaimTypes.Name, user.Username));
        claims.Add(new Claim(ClaimTypes.Sid, user.Id.ToString()));

        foreach (var role in user?.ApplicationRoles ?? new List<IApplicationRole>())
            claims.Add(new Claim(ClaimTypes.Role, role.GetType().Name));

        return new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
    }

    public async Task<ClaimsPrincipal> GetClaimsPrincipal()
    {
        return await Task.FromResult(_contextAccessor.HttpContext.User);
    }

    public async Task<bool> Login(string username, string password)
    {
        var user = await _security.ValidateUser(username, password);
        if (user == null)
            return false;

        await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, GetClaimsPrincipal(user),
            new AuthenticationProperties
            {
                IsPersistent = true
            });
        return true;
    }

    public async Task Logout()
    {
        await _contextAccessor.HttpContext.SignOutAsync();
    }

    internal class SessionIdentity
    {
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}