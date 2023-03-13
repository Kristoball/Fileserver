using Domain.Services;
using Fileserver.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fileserver.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlobProvider _blobProvider;

        public HomeController(IBlobProvider blobProvider)
        {
            _blobProvider = blobProvider;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var blobs = await _blobProvider.Blobs();

            var model = new FolderViewModel(blobs);
            return View(model);
        }
    }
}
