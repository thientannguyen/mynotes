using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace MyNotes.API.Authentication
{
    public class ApiKeyHandler : AuthorizationHandler<ApiKeyRequirement>
    {
        private IHttpContextAccessor _httpContextAccessor;

        public ApiKeyHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ApiKeyRequirement requirement)
        {
            string key = _httpContextAccessor.HttpContext.Request.Headers[requirement.ApiKeyName];
            if (!string.IsNullOrWhiteSpace(key) && key.Equals(requirement.ApiKeyValue, System.StringComparison.InvariantCultureIgnoreCase))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}