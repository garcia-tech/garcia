using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garcia.Application;
using Garcia.Application.Contracts.FileUpload;
using Garcia.Application.Contracts.Identity;
using Garcia.Application.Contracts.ImageResize;
using Garcia.Application.Contracts.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Garcia.Infrastructure.Api.Controllers
{
    public abstract class ApiController<TLoggedInUserModel, TKey> : ControllerBase
        where TLoggedInUserModel : IUser, new()
        where TKey : IEquatable<TKey>
    {
        protected GarciaInfrastructureApiSettings _settings;
        protected IAsyncRepository _repository;
        protected readonly IMediator _mediator;
        protected IFileUploadService _fileUploadService;
        protected IImageResizeService _imageResizeService;
        public string BaseUrl { get { return $"{Request.Scheme}://{Request.Host}{Request.PathBase}"; } }

        public ApiController(IOptions<GarciaInfrastructureApiSettings> settings, IMediator mediator, IAsyncRepository repository = null, IFileUploadService fileUploadService = null, IImageResizeService imageResizeService = null)
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

        public TLoggedInUserModel LoggedInApiUser
        {
            get
            {
                if (!string.IsNullOrEmpty(User.Identity.Name))
                {
                    return new TLoggedInUserModel() { Id = GetValueFromClaims<string>("id") };
                }

                return default(TLoggedInUserModel);
            }
        }

        public TKey UserId
        {
            get
            {
                return GetValueFromClaims<TKey>("id");
            }
        }

        public T GetValueFromClaims<T>(string claimsType)
        {
            if (User.Claims != null && !string.IsNullOrEmpty(claimsType))
            {
                var claims = User.Claims.FirstOrDefault(x => x.Type.ToLowerInvariant() == claimsType.ToLowerInvariant());

                if (claims != null)
                {
                    return Helpers.GetValueFromObject<T>(claims.Value);
                }
            }

            return default(T);
        }

        protected virtual string GetCultureCodeFromRequestHeaders()
        {
            return Request.Headers["Accept-Language"].ToString().Split(',').FirstOrDefault()?.Substring(0, 2);
        }
    }

    public class GarciaInfrastructureApiSettings
    {
        public string BaseImageUrl { get; set; }
    }

    public abstract class ApiController<TLoggedInUserModel> : ApiController<TLoggedInUserModel, int>
        where TLoggedInUserModel : IUser, new()
    {
        public ApiController(IOptions<GarciaInfrastructureApiSettings> settings, IMediator mediator, IAsyncRepository repository = null, IFileUploadService fileUploadService = null, IImageResizeService imageResizeService = null)
            : base(settings, mediator, repository, fileUploadService, imageResizeService)
        {
        }
    }
}
