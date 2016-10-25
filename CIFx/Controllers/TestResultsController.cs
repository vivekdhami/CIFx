using CIFx.Api.Models;
using CIFx.Api.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace CIFx.Api.Controllers
{
    [RoutePrefix("api/testresults")]
    public class TestResultsController : ApiController
    {
        private IBlobService blobService;

        public TestResultsController(IBlobService blobService)
        {
            this.blobService = blobService;
        }

        /// <summary>
        /// Uploads zipped test result files to azure blob storage.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(List<UploadedTestResult>))]
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Upload()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return StatusCode(HttpStatusCode.UnsupportedMediaType);
                }

                var result = await blobService.Upload(Request.Content);
                if (result != null && result.Count > 0)
                {
                    result.ForEach(i => i.FileUrl = $"{Request.RequestUri.GetLeftPart(UriPartial.Authority)}/api/testresults/{i.FileName}");

                    return Ok(result);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Downloads the zipped test result file or a file present inside the zipped file. 
        /// </summary>
        /// <param name="archiveName">The zipped file archive name.</param>
        /// <param name="fileName">The file name present inside the zipped file.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{archiveName}/{fileName?}")]
        public async Task<IHttpActionResult> Download(string archiveName, string fileName = "")
        {
            try
            {
                var result = await blobService.Download(archiveName, fileName);
                if (result == null)
                {
                    return NotFound();
                }

                var message = Request.CreateResponse(HttpStatusCode.OK);
                message.Content = new StreamContent(result.Stream);
                message.Content.Headers.ContentLength = result.FileSize;
                message.Content.Headers.ContentType = new MediaTypeHeaderValue(result.ContentType);
                message.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = HttpUtility.UrlDecode(result.FileName),
                    Size = result.FileSize
                };

                return ResponseMessage(message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Used by traffic manager to check endpoint health. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}
