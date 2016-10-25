using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace CIFx.Api.Tests.Helpers
{
    public class FakeControllerContextFactory
    {
        public static HttpControllerContext CreateMIMEContext()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "");
            request.RequestUri = new Uri("http://localhost:1278");
            var content = new MultipartFormDataContent();

            var fileContent = new ByteArrayContent(new Byte[100]);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "TestResult.zip"
            };
            content.Add(fileContent);
            request.Content = content;

            return new HttpControllerContext(new HttpConfiguration(), new HttpRouteData(new HttpRoute("")), request);
        }

        public static HttpControllerContext CreateTextContext()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "");
            request.Content = new StringContent("{\"name\":\"John Doe\",\"age\":33}",
                                    Encoding.UTF8,
                                    "application/json");

            return new HttpControllerContext(new HttpConfiguration(), new HttpRouteData(new HttpRoute("")), request);
        }

        public static HttpControllerContext CreateGetRequestContext()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "");
            return new HttpControllerContext(new HttpConfiguration(), new HttpRouteData(new HttpRoute("")), request);
        }
    }
}
