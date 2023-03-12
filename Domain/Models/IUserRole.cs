namespace Domain.Models;

public interface IUserRole
{
	IList<IApplicationRole> ApplicationRoles { get; }
}