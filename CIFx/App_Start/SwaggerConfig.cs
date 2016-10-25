using System.Web.Http;
using WebActivatorEx;
using CIFx.Api;
using Swashbuckle.Application;
using System;

//[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace CIFx.Api
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration httpConfig)
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            httpConfig
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "CIFx");
                        c.IncludeXmlComments(GetXmlCommentsPath());
                    })
                .EnableSwaggerUi(c =>
                    {
                    });
        }

        protected static string GetXmlCommentsPath()
        {            
            return string.Format(@"{0}\bin\CIFx.Api.XML", AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
