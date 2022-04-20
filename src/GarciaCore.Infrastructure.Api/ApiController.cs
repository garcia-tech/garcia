using GarciaCore.Application;
using GarciaCore.Application.Contracts.ImageResize;
using GarciaCore.Application.Contracts.FileUpload;
using GarciaCore.Application.Contracts.Persistence;
using GarciaCore.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GarciaCore.Infrastructure.Api
{
    public abstract class ApiController : ControllerBase
    {
        protected GarciaCoreInfrastructureApiSettings _settings;
        protected IAsyncRepository _repository;
        protected readonly IMediator _mediator;
        protected IFileUploadService _fileUploadService;
        protected IImageResizeService _imageResizeService;
        public string BaseUrl { get { return $"{Request.Scheme}://{Request.Host}{Request.PathBase}"; } }

        public ApiController(IOptions<GarciaCoreInfrastructureApiSettings> settings, IMediator mediator, IAsyncRepository repository = null, IFileUploadService fileUploadService = null, IImageResizeService imageResizeService = null)
        {
            _settings = settings?.Value;
            _repository = repository;
            _mediator = mediator;
            _fileUploadService = fileUploadService;
            _imageResizeService = imageResizeService;
        }

        protected IActionResult CreateResponse<T>(T item)
        {
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        protected bool ValidateResponse(bool response)
        {
            return response;
        }

        protected string CleanJson(string data)
        {
            return data.Replace("{}", "null").Replace("{ }", "null");
        }

        protected virtual async Task<List<UploadedFile>> MultipartUploadAsync()
        {
            if (_fileUploadService == null)
            {
                throw new Exception("FileUploadService is not injected");
            }

            var files = new List<UploadedFile>();

            if (Request.HasFormContentType)
            {
                var form = Request.Form;

                foreach (var formFile in form.Files)
                {
                    string newFileName = null;

                    if (_imageResizeService == null || _imageResizeService.ImageResizeSettings.PreserveOriginalFile)
                    {
                        var file = await _fileUploadService.MultipartUploadAsync(formFile);
                        newFileName = $"{file.FileName.Insert(file.FileName.LastIndexOf('.'), _imageResizeService.ImageResizeSettings.ResizedFileSuffix)}";

                        if (_imageResizeService == null)
                        {
                            file.FileName = _fileUploadService.GetUrl(file.FileName);
                            files.Add(file);
                        }
                    }

                    if (_imageResizeService != null)
                    {
                        var stream = _imageResizeService.ResizeToStream(formFile);

                        if (stream != null)
                        {
                            var file2 = await _fileUploadService.MultipartUploadAsync(stream, formFile.Name, formFile.ContentType, newFileName);
                            file2.FileName = _fileUploadService.GetUrl(file2.FileName);
                            files.Add(file2);
                        }
                    }
                }
            }

            return files;
        }
    }

    public class GarciaCoreInfrastructureApiSettings
    {
        public string BaseImageUrl { get; set; }
    }
}
