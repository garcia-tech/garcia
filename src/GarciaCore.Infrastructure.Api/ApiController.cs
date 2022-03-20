using GarciaCore.Application.Contracts.Persistence;
using GarciaCore.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace GarciaCore.Infrastructure.Api
{
    public abstract class ApiController : ControllerBase
    {
        protected GarciaCoreInfrastructureApiSettings _settings;
        protected IAsyncRepository _repository;
        protected readonly IMediator _mediator;
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
    }

    public class GarciaCoreInfrastructureApiSettings
    {
        public string BaseImageUrl { get; set; }
    }
}
