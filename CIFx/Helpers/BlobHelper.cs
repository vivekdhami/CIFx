using CIFx.Api.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace CIFx.Api.Helpers
{
    public class BlobHelper : IBlobHelper
    {
        private IStorageConfiguration storageConfiguration;

        public BlobHelper(IStorageConfiguration storageConfiguration)
        {
            this.storageConfiguration = storageConfiguration;
        }

        public CloudBlobContainer GetBlobContainer()
        {
            var blobStorageAccount = CloudStorageAccount.Parse(storageConfiguration.CloudStorageConnectionString);
            var blobClient = blobStorageAccount.CreateCloudBlobClient();
            blobClient.DefaultRequestOptions = new BlobRequestOptions
            {
                RetryPolicy = new ExponentialRetry()
            };
            var blobContainer = blobClient.GetContainerReference(storageConfiguration.ContainerName);
            blobContainer.CreateIfNotExists();

            return blobContainer;
        }
    }
}