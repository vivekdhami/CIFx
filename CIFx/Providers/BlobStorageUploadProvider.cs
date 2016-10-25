using CIFx.Api.Helpers;
using CIFx.Api.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CIFx.Api.Providers
{
    public class BlobStorageUploadProvider : MultipartFileStreamProvider
    {
        private IBlobHelper blobHelper;

        public List<UploadedTestResult> UploadedTestResults { get; set; }

        public BlobStorageUploadProvider(IBlobHelper blobHelper) : base(Path.GetTempPath())
        {
            this.blobHelper = blobHelper;
            UploadedTestResults = new List<UploadedTestResult>();
        }

        public override async Task ExecutePostProcessingAsync()
        {
            foreach (var fileData in FileData)
            {
                // Remove double quotes around file name if any.
                var fileName = Path.GetFileName(fileData.Headers.ContentDisposition.FileName.Trim('"'));

                var blobContainer = blobHelper.GetBlobContainer();
                var blob = blobContainer.GetBlockBlobReference(fileName);

                blob.Properties.ContentType = fileData.Headers.ContentType.MediaType;

                using (var fs = File.OpenRead(fileData.LocalFileName))
                {
                    await blob.UploadFromStreamAsync(fs);
                }

                File.Delete(fileData.LocalFileName);

                var uploadedTestResult = new UploadedTestResult
                {
                    FileName = blob.Name,
                    BlobUrl = blob.Uri.AbsoluteUri
                };
                
                UploadedTestResults.Add(uploadedTestResult);
            }

            await base.ExecutePostProcessingAsync();
        }
    }
}