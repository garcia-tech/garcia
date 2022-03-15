using System.Linq;
using System.Threading.Tasks;
using GarciaCore.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GarciaCore.Infrastructure.Api.Filters
{
    public class ValidationFilter<T> : IAsyncActionFilter where T : ApiError, new()
    {
        protected T Response { get; } = new();

        public ValidationFilter()
        {
            Response.Title = "Request model state is invalid";
        }

        public virtual async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid)
            {
                await next();
                return;
            }

            var errorsInModalState = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage))
                .ToArray();

            foreach (var error in errorsInModalState)
            {
                Response.AddErrors($"{error.Key} is invalid: {error.Value?.FirstOrDefault()}");
            }

            context.Result = new BadRequestObjectResult(Response);
        }
    }
}
