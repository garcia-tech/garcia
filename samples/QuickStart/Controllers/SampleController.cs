using Garcia.Application.Services;
using Garcia.Infrastructure.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuickStart.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class SampleController : BaseController<IBaseService<Sample, SampleDto, long>, Sample, SampleDto, long>
{
    public SampleController(IBaseService<Sample, SampleDto, long> services) : base(services)
    {
    }
}

