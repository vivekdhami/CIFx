using CIFx.Api.Controllers;
using CIFx.Api.Models;
using CIFx.Api.Services;
using CIFx.Api.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace CIFx.Api.Tests.Controllers
{

    [TestClass]
    public class TestResultsControllerUnitTest
    {
        Mock<IBlobService> mockedService;

        [TestInitialize]
        public void Init()
        {
            mockedService = new Mock<IBlobService>();
        }

        [TestMethod]
        public async Task Upload_WhenContentNotMultiPartMIME_ReturnUnsuppportedMediaType()
        {
            // Arrange
            var testResultsController = new TestResultsController(mockedService.Object);
            testResultsController.ControllerContext = FakeControllerContextFactory.CreateTextContext();

            // Act
            var result = await testResultsController.Upload();

            // Assert
            Assert.AreEqual(HttpStatusCode.UnsupportedMediaType, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public async Task Upload_WhenServiceReturnsNull_ReturnBadRequest()
        {
            // Arrange
            var testResultsController = new TestResultsController(mockedService.Object);
            testResultsController.ControllerContext = FakeControllerContextFactory.CreateMIMEContext();
            mockedService.Setup(m => m.Upload(It.IsAny<HttpContent>()))
                         .Returns(Task.FromResult((List<UploadedTestResult>)null));

            // Act
            var result = await testResultsController.Upload();

            // Assert
            Assert.AreEqual(typeof(BadRequestResult), result.GetType());
        }

        [TestMethod]
        public async Task Upload_WhenServiceReturnsResult_ReturnOk()
        {
            // Arrange
            var testResultsController = new TestResultsController(mockedService.Object);
            testResultsController.ControllerContext = FakeControllerContextFactory.CreateMIMEContext();
            mockedService.Setup(m => m.Upload(It.IsAny<HttpContent>()))
                         .Returns(Task.FromResult(new List<UploadedTestResult> {
                             new UploadedTestResult
                             {
                                 BlobUrl = "MockBlobUrl",
                                 FileName = "MockTestFile",
                                 FileUrl = "MockFileUrl"
                             }
                         }));

            // Act
            var result = await testResultsController.Upload();

            // Assert
            Assert.AreEqual(typeof(OkNegotiatedContentResult<List<UploadedTestResult>>), result.GetType());
        }

        [TestMethod]
        public async Task Upload_WhenServiceReturnsException_ReturnInternalServerError()
        {
            // Arrange
            var testResultsController = new TestResultsController(mockedService.Object);
            testResultsController.ControllerContext = FakeControllerContextFactory.CreateMIMEContext();
            mockedService.Setup(m => m.Upload(It.IsAny<HttpContent>()))
                         .ThrowsAsync(new Exception());

            // Act
            var result = await testResultsController.Upload();

            // Assert
            Assert.AreEqual(typeof(ExceptionResult), result.GetType());
        }

        [TestMethod]
        public async Task Download_WhenResultNotFound_ReturnsNotFound()
        {
            // Arrange
            var testResultsController = new TestResultsController(mockedService.Object);
            mockedService.Setup(m => m.Download(It.IsAny<string>(), It.IsAny<string>()))
                         .Returns(Task.FromResult((TestResultFile)null));

            // Act
            var result = await testResultsController.Download("TestArchive.zip", "TestResult.txt");

            // Assert
            Assert.AreEqual(typeof(NotFoundResult), result.GetType());
        }

        [TestMethod]
        public async Task Download_WhenResultFound_ReturnsOkWithResult()
        {
            // Arrange
            var testResultsController = new TestResultsController(mockedService.Object);
            testResultsController.ControllerContext = FakeControllerContextFactory.CreateGetRequestContext();
            var resultFile = new TestResultFile
            {
                FileName = "MockFileName",
                FileSize = 12121,
                ContentType = "application/zip",
                Stream = new MemoryStream()
            };
            mockedService.Setup(m => m.Download(It.IsAny<string>(), It.IsAny<string>()))
                         .Returns(Task.FromResult(resultFile));

            // Act
            var result = await testResultsController.Download("TestArchive.zip", "TestResult.txt");

            // Assert
            Assert.AreEqual(typeof(ResponseMessageResult), result.GetType());
            Assert.AreEqual(HttpStatusCode.OK, ((ResponseMessageResult)result).Response.StatusCode);
        }

        [TestMethod]
        public async Task Download_WhenServiceReturnsException_ReturnInternalServerError()
        {
            // Arrange
            var testResultsController = new TestResultsController(mockedService.Object);
            mockedService.Setup(m => m.Download(It.IsAny<string>(), It.IsAny<string>()))
                         .ThrowsAsync(new Exception());

            // Act
            var result = await testResultsController.Upload();

            // Assert
            Assert.AreEqual(typeof(ExceptionResult), result.GetType());
        }
    }
}
