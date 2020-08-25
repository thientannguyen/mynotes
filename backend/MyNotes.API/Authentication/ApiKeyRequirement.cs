using Microsoft.AspNetCore.Authorization;

namespace MyNotes.API.Authentication
{
    public class ApiKeyRequirement : IAuthorizationRequirement
    {
        public string ApiKeyName { get; }
        public string ApiKeyValue { get; }

        public ApiKeyRequirement(string apiKeyName, string apiKeyValue)
        {
            ApiKeyName = apiKeyName;
            ApiKeyValue = apiKeyValue;
        }
    }
}