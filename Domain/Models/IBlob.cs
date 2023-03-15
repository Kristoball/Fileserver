namespace Domain.Models;

public interface IBlob
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public Guid ParentId { get; set; }
    public string FileType { get; }
}
