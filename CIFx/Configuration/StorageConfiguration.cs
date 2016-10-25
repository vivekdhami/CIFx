using System.Configuration;

namespace CIFx.Api.Configuration
{
    public class StorageConfiguration : IStorageConfiguration
    {
        const string BlobContainerName = "BlobContainerName";
        const string StorageConnectionString = "StorageConnectionString";

        public string ContainerName
        {
            get
            {
                return ConfigurationManager.AppSettings[BlobContainerName];
            }
        }

        public string CloudStorageConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings[StorageConnectionString];
            }
        }
    }
}