using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services;

public interface IBlobProvider
{

    Task<IBlob> Blob(Guid id);
    Task<IEnumerable<IBlob>> Blobs(Guid? parentFolder = null);
    Task<IBlob> AddBlob(IBlob folder);
    Task EditBlob(IBlob folder);
    Task DeleteBlob(Guid id);
    Task DownloadBlob(IBlob blob);
    Task<string> GetPath(Guid blobId);
    Task<IList<IBlob>> GetChildren(Guid id);
}
