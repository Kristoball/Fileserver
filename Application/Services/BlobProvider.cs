using Domain.Models;
using Domain.Services;
using System.Security.Claims;

namespace Infrastructure.Services;

public class BlobProvider : IBlobProvider
{
    private readonly IAuthenticationStateProvider _authenticationStateProvider;
    private IList<IBlob> _blobs = new List<IBlob>();
    public BlobProvider(IAuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }
    public async Task<IBlob> Blob(Guid id)
    {
        return await Task.FromResult(_blobs.SingleOrDefault(x => x.Id == id));
    }

    public async Task<IEnumerable<IBlob>> Blobs(Guid? parentFolder = null)
    {
        var user = await _authenticationStateProvider.GetClaimsPrincipal();
        var userId = new Guid(user.Claims.First(x => x.Type == ClaimTypes.Sid).Value);
        if (parentFolder != null)
        {
            return await Task.FromResult(_blobs.Where(x => x.ParentId == parentFolder));
        }

        return await Task.FromResult(
            _blobs.Where(x => x is IFolder && (x as IFolder)?.OwnerUserId == userId));
    }

    public async Task<IBlob> AddBlob(IBlob folder)
    {
        _blobs.Add(folder);
        return await Task.FromResult(folder);
    }

    public Task DeleteBlob(IBlob blob)
    {
        throw new NotImplementedException();
    }

    public Task DownloadBlob(IBlob blob)
    {
        throw new NotImplementedException();
    }

    public Task EditBlob(IBlob folder)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetPath(Guid blobId)
    {
        var path = blobId.ToString();
        var blob = blobId;
        while (blob != Guid.Empty)
        {
            path = $"{blob}/{path}";
            blob = _blobs.First(x => x.Id == blob).ParentId;
        }

        path = (await _authenticationStateProvider.GetClaimsPrincipal()).Claims.First(x => x.Type == ClaimTypes.Sid).Value + path;

        return path;
    }
}
