using Domain.Models;

namespace Fileserver.ViewModels.Home;

public class FolderViewModel
{
    public FolderViewModel(IBlob current, IEnumerable<IBlob> blobs, bool canAccessParent)
    {
        Current = current;
        Blobs = blobs;
        CanAccessParent = canAccessParent;
    }

    public FolderViewModel(IEnumerable<IBlob> blobs)
    {
        Blobs = blobs;
    }

    public bool CanAccessParent { get; set; } = false;
    public IBlob? Current { get; set; }
    public IEnumerable<IBlob> Blobs { get; set; } = new List<IBlob>();
}
