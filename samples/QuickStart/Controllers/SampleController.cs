using Garcia.Application.Services;
using Garcia.Infrastructure.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuickStart.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class SampleController : BaseController<Sample, SampleDto>
{
    public SampleController(IBaseService<Sample, SampleDto, long> services) : base(services)
    {
    }
}

