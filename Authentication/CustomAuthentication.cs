using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace ProjectName.Authentication;

public class CustomAuthentication(ProtectedSessionStorage sessionStorage) : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _guest = new(new ClaimsIdentity());

    // Get the authentication out of the Sessionstorage
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSessionResult = await sessionStorage.GetAsync<UserSession>("UserSession");
            var userSession = userSessionResult.Success ? userSessionResult.Value : null;

            if (userSession == null) return await Task.FromResult(new AuthenticationState(_guest));
                ClaimsPrincipal claimsPrincipal = new(new ClaimsIdentity(new List<Claim>
                {
                    new(ClaimTypes.Email, userSession.Email),
                    new(ClaimTypes.Role, userSession.Role)
                },
                    "CustomAuth"
                ));
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        catch
        {
            return await Task.FromResult(new AuthenticationState(_guest));
        }
    }

    // Updates the authentication and set it in the Sessionstorage
    public async Task UpdateAuthenticationState(UserSession? userSession)
    {
        ClaimsPrincipal claimsPrincipal;

        if (userSession != null)
        {
            await sessionStorage.SetAsync("UserSession", userSession);
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> {
                new(ClaimTypes.Email , userSession.Email),
                new(ClaimTypes.Role, userSession.Role)
            }));

        }
        else
        {
            await sessionStorage.DeleteAsync("UserSession");
            claimsPrincipal = _guest;
        }
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
}