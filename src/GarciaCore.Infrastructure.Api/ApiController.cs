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
        public string BaseUrl { get { return $"{Request.Scheme}://{Request.Host}{Request.PathBase}"; } }

        public ApiController(IOptions<GarciaCoreInfrastructureApiSettings> settings, IAsyncRepository repository, IMediator mediator)
        {
            _settings = settings?.Value;
            _repository = repository;
            _mediator = mediator;
        }

        public ApiController(IOptions<GarciaCoreInfrastructureApiSettings> settings, IMediator mediator) : this(settings, null, mediator)
        {
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

        protected async Task<List<string>> MultipartUploadAsync()
        {
            if (_fileUploadService == null)
            {
                throw new Exception("FileUploadService is not injected");
            }

            var files = new List<string>();

            if (Request.HasFormContentType)
            {
                var form = Request.Form;

                foreach (var formFile in form.Files)
                {
                    var file = await _fileUploadService.MultipartUploadAsync(formFile);
                    files.Add(_fileUploadService.GetUrl(file.FileName));
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
