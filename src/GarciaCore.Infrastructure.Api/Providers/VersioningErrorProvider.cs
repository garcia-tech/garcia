using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GarciaCore.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace GarciaCore.Infrastructure.Api.Providers
{
    public class VersioningErrorProvider<T> : DefaultErrorResponseProvider where T : ApiError, new()
    {
        private T Response { get; } = new();
        public VersioningErrorProvider()
        {
            Response.Title = "API Version does not supported";
        }

        public VersioningErrorProvider(string errorTitle)
        {
            Response.Title = errorTitle;
        }

        public override IActionResult CreateResponse(ErrorResponseContext context)
        {

            switch (context.ErrorCode)
            {
                case "UnsupportedApiVersion":
                    Response.AddErrors(context.ErrorCode);
                    return new BadRequestObjectResult(Response);
                default:
                    break;
            }
            return base.CreateResponse(context);
        }
    }
}
