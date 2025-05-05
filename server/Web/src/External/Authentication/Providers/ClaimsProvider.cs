using System.Security.Claims;
using System.Text.Json;
using Authentication.Abstractions;
using Authentication.Constants;
using Domain.UserAggregate;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Providers;

internal sealed class ClaimsProvider(
    UserManager<User> userManager) : IClaimsProvider
{
    public async Task<Claim[]> GetClaimsAsync(User user)
    {
        var roles = await userManager.GetRolesAsync(user);

        var role = roles.Contains(UserRoleNames.Admin) ? UserRoleNames.Admin : UserRoleNames.User;

        var claims = new List<Claim>()
        {
            new(JwtClaimTypes.UserId, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(JwtClaimTypes.Name, $"{user.FirstName.Value} {user.LastName.Value}"),
            new(ClaimTypes.Role, role)
        };

        return claims.ToArray();
    }
}