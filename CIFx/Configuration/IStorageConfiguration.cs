namespace CIFx.Api.Configuration
{
    public interface IStorageConfiguration
    {
        string CloudStorageConnectionString { get; }

        string ContainerName { get; }
    }
}
