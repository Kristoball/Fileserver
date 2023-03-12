using Domain.Models;

namespace Application.Models
{
    public class MenuItem : IMenuItem
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();
        public MenuItem AddRole(IApplicationRole role)
        {
            Roles.Add(role.GetType().Name);
            return this;
        }
    }
}
