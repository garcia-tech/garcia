using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GarciaCore.Infrastructure.Api.Controllers
{
    [ApiController]
    public abstract class ErrorController : ControllerBase
    {
        protected virtual IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var statusCode = CreateStatusCode(exception);
            var message = CreateErrorMessage(exception);
            return Problem(message, statusCode: statusCode);
        }

        protected abstract string CreateErrorMessage(IExceptionHandlerFeature exception);
        protected abstract int CreateStatusCode(IExceptionHandlerFeature exception);
    }
}
