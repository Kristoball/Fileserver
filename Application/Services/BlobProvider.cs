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
        var userId = await _authenticationStateProvider.GetId();
        if (parentFolder != null)
        {
            return await Task.FromResult(_blobs.Where(x => x.ParentId == parentFolder));
        }

        return await Task.FromResult(
            _blobs.Where(x => x is IFolder && (x as IFolder)?.OwnerUserId == userId && x.ParentId == Guid.Empty));
    }

    public async Task<IBlob> AddBlob(IBlob folder)
    {
        _blobs.Add(folder);
        return await Task.FromResult(folder);
    }

    public async Task DeleteBlob(Guid id)
    {
        var blobsToRemoveFound = _blobs.Where(x => x.ParentId == id).ToList();
        var blobsToRemove = new List<IBlob>();
        while (blobsToRemoveFound.Any())
        {
            var blob = blobsToRemoveFound.First();
            if (blob is IFolder)
                blobsToRemoveFound.AddRange(_blobs.Where(x => x.ParentId == blob.Id));
            blobsToRemove.Add(blob);
            blobsToRemoveFound.Remove(blob);
        }

        foreach(var blob in blobsToRemove)
            _blobs.Remove(blob);

        _blobs.Remove(_blobs.Single(x => x.Id == id));
        await Task.CompletedTask;
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

        path = (await _authenticationStateProvider.GetId()) + path;

        return path;
    }

    public async Task<IList<IBlob>> GetChildren(Guid id)
    {
        List<IBlob> children = new List<IBlob>();
        var x = _blobs.Where(x => x.ParentId == id).ToList();
        while (x.Any())
        {
            var blob = x.First();
            children.Add(blob);
            x.AddRange(_blobs.Where(x=>x.ParentId == blob.Id));
            x.Remove(blob);
        }

        return await Task.FromResult(children);
    }
}
