using CIFx.Api.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CIFx.Api.Services
{
    public interface IBlobService
    {
        Task<List<UploadedTestResult>> Upload(HttpContent httpContent);

        Task<TestResultFile> Download(string blobName, string fileName);
    }
}
