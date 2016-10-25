using Microsoft.WindowsAzure.Storage.Blob;

namespace CIFx.Api.Helpers
{
    public interface IBlobHelper
    {
        CloudBlobContainer GetBlobContainer();
    }
}