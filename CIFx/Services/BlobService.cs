using CIFx.Api.Helpers;
using CIFx.Api.Models;
using CIFx.Api.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace CIFx.Api.Services
{
    public class BlobService : IBlobService
    {
        private IBlobHelper blobHelper;

        public BlobService(IBlobHelper blobHelper)
        {
            this.blobHelper = blobHelper;
        }

        public async Task<List<UploadedTestResult>> Upload(HttpContent httpContent)
        {
            var blobUploadProvider = new BlobStorageUploadProvider(blobHelper);

            await httpContent.ReadAsMultipartAsync(blobUploadProvider);

            return blobUploadProvider.UploadedTestResults;
        }

        public async Task<TestResultFile> Download(string blobName, string fileName)
        {
            var container = blobHelper.GetBlobContainer();
            var blob = container.GetBlockBlobReference(blobName);

            var ms = await blob.OpenReadAsync();

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                ZipArchive zipArchive = new ZipArchive(ms);
                foreach (ZipArchiveEntry entry in zipArchive.Entries)
                {
                    if (entry.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                    {
                        var fileStream = entry.Open();

                        return new TestResultFile
                        {
                            Stream = fileStream,
                            FileName = fileName,
                            FileSize = entry.Length,
                            ContentType = blob.Properties.ContentType
                        };
                    }
                }
                throw new FileNotFoundException($"File: {fileName} not found.");
            }
            
            var download = new TestResultFile
            {
                Stream = ms,
                FileName = blob.Name,
                FileSize = blob.Properties.Length,
                ContentType = blob.Properties.ContentType
            };

            return download;
        }
    }
}