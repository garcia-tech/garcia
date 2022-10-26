using System;
using System.Threading.Tasks;
using Garcia.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Garcia.Infrastructure.Api.Middlewares.Exceptions
{
    /// <summary>
    /// When an exception is thrown, automatically executes if there is no catch block for that exception.
    /// In case of exception returns <typeparamref name="TErrorModel"/> to the client.
    /// </summary>
    /// <typeparam name="TErrorModel"></typeparam>
    public class ExceptionHandler<TErrorModel> : IMiddleware where TErrorModel : ApiError, new()
    {
        private readonly ILogger<ExceptionHandler<TErrorModel>> _logger;
        private readonly IOptions<ExceptionHandlingOptions> _options;

        public ExceptionHandler(ILogger<ExceptionHandler<TErrorModel>> logger, IOptions<ExceptionHandlingOptions> options)
        {
            _logger = logger;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
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
            errorModel.AddError(exception.Message);
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
