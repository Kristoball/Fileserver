using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services;

public interface IBlobProvider
{
    public Task<IEnumerable<IBlob>> Blobs(IFolder? parentFolder = null);
    public Task<IFolder> CreateFolder(IFolder folder);
    public Task EditBlob(IBlob folder);
    public Task DeleteBlob(IBlob blob);
    public Task DownloadBlob(IBlob blob);
}
