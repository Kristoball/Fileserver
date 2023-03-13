using Domain.Models;
using Domain.Services;
using System.Security.Claims;

namespace Application.Services;

public class BlobProvider : IBlobProvider
{
    private readonly IAuthenticationStateProvider _authenticationStateProvider;
    private IList<IBlob> _blobs = new List<IBlob>();
    public BlobProvider(IAuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }
    public async Task<IEnumerable<IBlob>> Blobs(IFolder? parentFolder = null)
    {
        var user = await _authenticationStateProvider.GetClaimsPrincipal();
        return await Task.FromResult(
            _blobs.Where(x => x is IFolder && (x as IFolder)?.OwnerUserId == new Guid(user.Claims.First(x => x.Type == ClaimTypes.Sid).Value)));
    }

    public Task<IFolder> CreateFolder(IFolder folder)
    {
        throw new NotImplementedException();
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
}
