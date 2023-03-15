using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models;

public class Blob : IBlob
{
    public Blob(string fileName, Guid parentId, string createdBy, string fileType)
    {
        Name = fileName;
        Id = Guid.NewGuid();
        Created = DateTime.UtcNow;
        ParentId = parentId;
        CreatedBy = createdBy;
        FileType = fileType;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public Guid ParentId { get; set; }
    public string FileType { get; set; }
}
