using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static Garcia.Infrastructure.Identity.Constants.Strings;

namespace Garcia.Infrastructure.Identity
{
    [AttributeUsage(AttributeTargets.All)]
    public class AuthorizeRoleAttribute : Attribute, IAuthorizationFilter
    {
        protected string[] Roles { get; }

        public AuthorizeRoleAttribute(params string[] roles)
        {
            Roles = roles;
        }

        public virtual void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var roles = user.Claims.Where(x => x.Type == JwtClaimIdentifiers.Role).Select(x => x.Value);

            if (!Roles.Any(x => roles.Contains(x)))
                context.Result = new ForbidResult();
        }
    }
}
