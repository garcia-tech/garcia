using Microsoft.AspNetCore.Authorization;

namespace Garcia.Infrastructure.Api.Providers
{
    internal class AuthorizeData : IAuthorizeData
    {
        public string Policy { get; set; }
        public string Roles { get; set; }
        public string AuthenticationSchemes { get; set; }
    }
}
