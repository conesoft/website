using Microsoft.AspNetCore.Components.Authorization;

namespace Conesoft.Website.Utilities;

public static class AuthenticationStateHelpers
{
    public static async Task<string?> GetUserName(this Task<AuthenticationState>? authenticationState)
    {
        if (authenticationState != null)
        {
            var state = await authenticationState;
            return state?.User?.Identity?.Name;
        }
        return null;
    }
}