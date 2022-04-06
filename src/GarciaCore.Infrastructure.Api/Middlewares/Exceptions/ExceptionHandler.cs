using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarciaCore.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GarciaCore.Infrastructure.Api.Middlewares.Exceptions
{
    public class ExceptionHandler<TErrorModel> where TErrorModel : ApiError, new()
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IOptions<ExceptionHandlingOptions> _options;

        public ExceptionHandler(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public ExceptionHandler(RequestDelegate next, ILogger logger, IOptions<ExceptionHandlingOptions> options)
        {
            _next = next;
            _logger = logger;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await Handle(context, exception);
            }
        }

        private async Task Handle(HttpContext context, Exception exception)
        {
            var errorModel = new TErrorModel
            {
                Title = _options?.Value.DefaultExceptionTitle
            };

            errorModel.SetStatusCode(System.Net.HttpStatusCode.InternalServerError);
            errorModel.AddErrors(exception.Message);
            SetError(errorModel, exception);
            context.Response.ContentType = _options?.Value.ResponseContentType ?? "application/json";
            context.Response.StatusCode = errorModel.StatusCode.Value;
            await LogError(errorModel, context, exception);
            await context.Response.WriteAsync(errorModel.ToString());
        }

        protected virtual void SetError(TErrorModel error, Exception exception)
        {
        }

        protected virtual async Task LogError(TErrorModel errorModel, HttpContext context, Exception exception)
        {
            _logger.LogError(errorModel.ToString());
            await Task.CompletedTask;
        }
    }
}
