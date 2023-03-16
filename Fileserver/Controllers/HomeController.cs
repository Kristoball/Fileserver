using Domain.Services;
using Infrastructure.Models;
using Fileserver.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.Models;

namespace Fileserver.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IBlobProvider _blobProvider;
        private readonly IHashingAlgorithm _hashingAlgorithm;
        private readonly IAuthenticationStateProvider _authenticationStateProvider;
        private readonly IFilestore _filestore;

        public HomeController(IBlobProvider blobProvider, IHashingAlgorithm hashingAlgorithm, IAuthenticationStateProvider authenticationStateProvider, IFilestore filestore)
        {
            _blobProvider = blobProvider;
            _hashingAlgorithm = hashingAlgorithm;
            _authenticationStateProvider = authenticationStateProvider;
            _filestore = filestore;
        }

        [Authorize]
        [HttpGet("/{folderId?}")]
        public async Task<IActionResult> Index(Guid? folderId)
        {
            var blobs = await _blobProvider.Blobs(folderId);
            if(!folderId.HasValue)
                return View(new FolderViewModel(blobs));

            var parent = await _blobProvider.Blob(folderId.Value);

            return View(new FolderViewModel(parent, blobs, true));
        }

        [HttpPost]
        public async Task<IActionResult> AddFolder(string folderName, string password, Guid parentFolder)
        {
            var user = await _authenticationStateProvider.GetClaimsPrincipal();
            var userId = new Guid(user.Claims.First(x => x.Type == ClaimTypes.Sid).Value);
            var folder = await _blobProvider.AddBlob(new Folder(folderName, _hashingAlgorithm.HashString(password), userId, parentFolder, user.Identity.Name));
            
            return RedirectToAction("Index", new { folderId = folder.Id});
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(List<IFormFile> files, Guid parentFolder)
        {
            var user = await _authenticationStateProvider.GetClaimsPrincipal();
            var userId = new Guid(user.Claims.First(x => x.Type == ClaimTypes.Sid).Value);
            foreach(var file in files)
            {
                var blob = new Blob(file.FileName, parentFolder, user?.Identity?.Name ?? "Guest", file.ContentType);
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                ms.Position = 0;
                await _blobProvider.AddBlob(blob);
                await _filestore.Upload(ms, await _blobProvider.GetPath(blob.Id));
                
            }

            return RedirectToAction("Index", new { folderId = parentFolder });
        }

        [HttpGet]
        public async Task<IActionResult> GetFile(Guid id, bool download = true)
        {
            var ms = new MemoryStream();
            await _filestore.Get(await _blobProvider.GetPath(id), ms);
            ms.Position = 0;

            var blob = await _blobProvider.Blob(id);
            if (download)
            {
                return File(ms, blob.FileType, blob.Name);
            }
            else
            {
                return File(ms, blob.FileType);
            }
        }

        public async Task<IActionResult> VerifyDelete(Guid id)
        {
            var blob = await _blobProvider.Blob(id);
            return View(blob);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var blob = await _blobProvider.Blob(id);
            Guid? parent = blob.ParentId == Guid.Empty ? null : blob.ParentId;

            //figured the responsibillity to delete all files was in the services
            //if (blob is IFolder)
            //{
            //    var children = await _blobProvider.GetChildren(id);
            //    var files = children.Where(x => x is not IFolder);

            //    var listOfTasks = new List<Task>();
            //    foreach (var file in files)
            //    {
            //        listOfTasks.Add(_filestore.Delete(await _blobProvider.GetPath(file.Id)));
            //        listOfTasks.Add(_blobProvider.DeleteBlob(id));
            //    }

            //    foreach (var folder in children.Where(x=>x is IFolder))
            //        listOfTasks.Add(_blobProvider.DeleteBlob(id));

            //    await Task.WhenAll(listOfTasks);
            //    await _blobProvider.DeleteBlob(id);
            //} 
            //else
            //{
            //    await _filestore.Delete(await _blobProvider.GetPath(id));
            //    await _blobProvider.DeleteBlob(id);
            //}


            await _filestore.Delete(await _blobProvider.GetPath(id));
            await _blobProvider.DeleteBlob(id);

            return RedirectToAction("index", new {folderId=parent});
        }
    }
}
