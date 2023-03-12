using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models;
public interface IUser
{
	IDictionary<string, string> Preferences { get; set; }
	string Email { get; set; }
	IEnumerable<IApplicationRole> ApplicationRoles { get; }
	IEnumerable<IUserRole> CustomerRoles { get; set; }
	string Username { get; set; }
	bool Active { get; set; }
	bool CheckPassword(IHashingAlgorithm algorithm, string password);

}