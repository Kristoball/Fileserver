using Domain.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fileserver.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IAuthenticationStateProvider _authenticationStateProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationSecurity _applicationSecurity;

    public AccountController(IAuthenticationStateProvider authenticationStateProvider, IHttpContextAccessor httpContextAccessor, IApplicationSecurity applicationSecurity)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _httpContextAccessor = httpContextAccessor;
        _applicationSecurity = applicationSecurity;
    }

    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public async Task<IActionResult> Login()
    {
        if((await _authenticationStateProvider.GetClaimsPrincipal()).Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
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

    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(string userName, string name, string password)
    {
        await _applicationSecurity.RegisterUser(userName, name, password);
        return RedirectToAction("Login", "Account");
    }
}
