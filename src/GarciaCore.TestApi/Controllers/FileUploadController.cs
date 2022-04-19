using GarciaCore.Application;
using GarciaCore.Application.Contracts.FileUpload;
using GarciaCore.Application.Contracts.ImageResize;
using GarciaCore.Application.Contracts.Persistence;
using GarciaCore.Infrastructure.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GarciaCore.TestApi.Controllers
{
    public class FileUploadController : ApiController
    {
        public FileUploadController(IOptions<GarciaCoreInfrastructureApiSettings> settings, IMediator mediator, IFileUploadService fileUploadService, IImageResizeService imageResizeService) : base(settings, mediator)
        {
            _fileUploadService = fileUploadService;
            _imageResizeService = imageResizeService;
        }

        [HttpPost("api/FileUpload", Name = "FileUpload")]
        public async Task<ActionResult> CreateDiseaseRecognitionWithMultipartUpload()
        {
            var files = await MultipartUploadAsync();
            return Ok();
        }
    }
}
