using System.Security.Claims;
using Application.Authentication.UserContext;
using Authentication.Constants;
using Domain.UserAggregate;
using Microsoft.AspNetCore.Http;

namespace Authentication.Providers;

internal sealed class UserContextProvider(
    IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser? GetCurrentUser()
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user?.Identity == null) return null;
        if (!user.Identity.IsAuthenticated) return null;

        var id = user.FindFirst(JwtClaimTypes.UserId)!.Value;
        var email = user.FindFirst(ClaimTypes.Email)!.Value;
        var name = user.FindFirst(JwtClaimTypes.Name)!.Value;
        var role = user.FindFirst(ClaimTypes.Role)!.Value;

        return new CurrentUser
        {
            Id = id,
            Email = email,
            FirstName = name.Split(' ')[0],
            LastName = name.Split(' ')[1],
            Role = UserRole.FromName(role)!
        };
    }
}