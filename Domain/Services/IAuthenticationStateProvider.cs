using System.Security.Claims;

namespace Domain.Services;
public interface IAuthenticationStateProvider
{
	Task<bool> Login(string username, string password);
	Task Logout();
	Task<ClaimsPrincipal> GetClaimsPrincipal();
}
