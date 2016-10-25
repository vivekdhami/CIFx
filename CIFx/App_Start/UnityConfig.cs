using CIFx.Api.Configuration;
using CIFx.Api.Helpers;
using CIFx.Api.Services;
using Microsoft.Practices.Unity;
using System.Web.Http;

namespace CIFx.Api
{
    public class UnityConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<IStorageConfiguration, StorageConfiguration>();
            container.RegisterType<IBlobHelper, BlobHelper>();
            container.RegisterType<IBlobService, BlobService>();

            config.DependencyResolver = new UnityResolver(container);
        }
    }
}