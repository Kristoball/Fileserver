using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services;

public interface IBlobProvider
{

    public Task<IBlob> Blob(Guid id);
    public Task<IEnumerable<IBlob>> Blobs(Guid? parentFolder = null);
    public Task<IBlob> AddBlob(IBlob folder);
    public Task EditBlob(IBlob folder);
    public Task DeleteBlob(IBlob blob);
    public Task DownloadBlob(IBlob blob);
    public Task<string> GetPath(Guid blobId);
}
