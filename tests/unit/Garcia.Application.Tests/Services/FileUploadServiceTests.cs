using System.Threading.Tasks;
using Garcia.Application.Contracts.FileUpload;
using Garcia.Infrastructure.FileUpload.Local;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Xunit;

namespace Garcia.Application.Tests.Services
{
    public class FileUploadServiceTests
    {
        private static readonly Mock<IOptions<LocalFileUploadSettings>> _mockOptions = new();
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
