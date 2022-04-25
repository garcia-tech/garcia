using Garcia.Application.Contracts.FileUpload;
using Garcia.Application.Contracts.ImageResize;
using Garcia.Application;
using Garcia.Application.Contracts.Persistence;
using Garcia.Infrastructure.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Garcia.TestApi.Controllers
{
    public class FileUploadController : ApiController
    {
        public FileUploadController(IOptions<GarciaInfrastructureApiSettings> settings, IMediator mediator, IFileUploadService fileUploadService, IImageResizeService imageResizeService) : base(settings, mediator)
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
