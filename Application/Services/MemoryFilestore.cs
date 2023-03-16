using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class MemoryFilestore : IFilestore
    {
        private IList<FileStoreFile> _files = new List<FileStoreFile>();
        
        public async Task Delete(string path)
        {
            var id = path.Split('/').Last();
            var filesToRemove = _files.Where(x => x.Path.Contains(id)).ToList();
            foreach (var file in filesToRemove)
                _files.Remove(file);
            
            await Task.CompletedTask;
        }

        public async Task Get(string path, MemoryStream streamToCopyTo)
        {
            var file = _files.SingleOrDefault(x=>x.Path == path);
            if (file != null)
            {
                file.File.Position = 0;
                streamToCopyTo.Position = 0;
                await file.File.CopyToAsync(streamToCopyTo);
                streamToCopyTo.Position = 0;
                file.File.Position = 0;
            }
            await Task.CompletedTask;
        }

        public async Task Upload(MemoryStream file, string path)
        {
            _files.Add(new FileStoreFile(file, path));
            await Task.CompletedTask;
        }

        private class FileStoreFile
        {
            public FileStoreFile(MemoryStream file, string path)
            {
                file.Position = 0;
                file.CopyTo(File);
                file.Position = 0;
                File.Position = 0;
                Path = path;
            }

            public MemoryStream File { get; } = new MemoryStream();
            public string Path { get; }
        }
    }
}
