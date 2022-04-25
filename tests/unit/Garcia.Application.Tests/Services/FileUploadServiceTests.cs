using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Garcia.Application.Contracts.Persistence;
using Garcia.Application.Services;
using Moq;
using Shouldly;
using Microsoft.Extensions.Options;
using Garcia.Application.Contracts.FileUpload;
using Garcia.Infrastructure.FileUpload.Local;

namespace Garcia.Application.Tests.Services
{
    public class FileUploadServiceTests
    {
        private static Mock<IOptions<LocalFileUploadSettings>> _mockOptions = new();
        private readonly IFileUploadService _service;

        public FileUploadServiceTests()
        {
            var settings = new LocalFileUploadSettings
            {
              BaseUrl = "http://baseurl/",
              FileUploadPath = "c:\\files\\"
            };

            _mockOptions.Setup(s => s.Value).Returns(settings);
            _service = new LocalFileUploadService(_mockOptions.Object);
        }

        [Fact]
        public async Task Base64Upload_Success()
        {
            var response = await _service.Base64UploadAsync("testfile.jpg", "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABAQMAAAAl21bKAAAAA1BMVEUAAACnej3aAAAAAXRSTlMAQObYZgAAAApJREFUCNdjYAAAAAIAAeIhvDMAAAAASUVORK5CYII=");
            response.ShouldNotBeNull();
        }
    }
}
