namespace Domain.Models;

public interface IFolder : IBlob
{
    public string Password { get; set; }
    public DateTime Expires { get; set; }
    public bool IsDeleted { get; set; }
    public Guid OwnerUserId { get; set; }
}