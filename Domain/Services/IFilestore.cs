using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services;

public interface IFilestore
{
    public Task Upload(MemoryStream file, string path);
    public Task Get(string path, MemoryStream streamToCopyTo);
    public Task Delete(string path);
}
