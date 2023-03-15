using Domain.Models;

namespace Infrastructure.Models;

public class Folder : IFolder
{
    public Folder(string folderName, string password, Guid ownerId, Guid parentId, string createdBy)
    {
        Name = folderName;
        Password = password;
        Id = Guid.NewGuid();
        Created = DateTime.UtcNow;
        OwnerUserId = ownerId;
        ParentId = parentId;
        CreatedBy = createdBy;
    }
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
    public string FileType => "folder";
}
