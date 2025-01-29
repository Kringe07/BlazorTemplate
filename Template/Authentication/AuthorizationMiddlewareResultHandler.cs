using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace ProjectName.Authentication;

public class AuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    // Needed because of a .net 8 blazor template bug 
    // https://stackoverflow.com/questions/77693596/unexpected-authorization-behaviour-in-a-blazor-web-app-with-net-8
    public Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        return next(context);
    }
}