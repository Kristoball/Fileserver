using Domain.Models;

namespace Application.Models;

internal class Folder : IFolder
{
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public DateTime Expires { get; set; }
    public bool IsDeleted { get; set; }
    public Guid Id { get; set; }
    public string CreatedBy { get; set; } = String.Empty;
    public DateTime Created { get; set; }
    public Guid OwnerUserId { get; set; }
    public Guid ParentId { get; set; }
}
