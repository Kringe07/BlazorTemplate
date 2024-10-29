using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace ProjectName.Authentication
{
    public class CustomAuthentication : AuthenticationStateProvider
    {
        public readonly ProtectedSessionStorage _sessionstorage;

        private ClaimsPrincipal _guest = new(new ClaimsIdentity());

        public CustomAuthentication(ProtectedSessionStorage sessionStorage)
        {
            _sessionstorage = sessionStorage;
        }

        // Get the authentication out of the Sessionstorage
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                ProtectedBrowserStorageResult<UserSession> userSessionResult = await _sessionstorage.GetAsync<UserSession>("UserSession");
                UserSession? userSession = userSessionResult.Success ? userSessionResult.Value : null;

                if (userSession == null) return await Task.FromResult(new AuthenticationState(_guest));
                ClaimsPrincipal ClaimsPrincipal = new(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, userSession.Email),
                        new Claim(ClaimTypes.Role, userSession.Role)
                    },
                    "CustomAuth"
                ));
                return await Task.FromResult(new AuthenticationState(ClaimsPrincipal));
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
                await _sessionstorage.SetAsync("UserSession", userSession);
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> {
                new Claim(ClaimTypes.Email , userSession.Email),
                new Claim (ClaimTypes.Role, userSession.Role)
                }));

            }
            else
            {
                await _sessionstorage.DeleteAsync("UserSession");
                claimsPrincipal = _guest;
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}
