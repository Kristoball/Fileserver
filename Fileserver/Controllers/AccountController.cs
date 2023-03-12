using Domain.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fileserver.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IAuthenticationStateProvider _authenticationStateProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountController(IAuthenticationStateProvider authenticationStateProvider, IHttpContextAccessor httpContextAccessor)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _httpContextAccessor = httpContextAccessor;
    }
    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(string userName, string password)
    {
        var login = await _authenticationStateProvider.Login(userName, password);
        if (!login)
            return RedirectToAction("Login", "Account");
        
        return RedirectToAction("Index", "Home");
    }
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _authenticationStateProvider.Logout();
        _httpContextAccessor?.HttpContext.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }
}
