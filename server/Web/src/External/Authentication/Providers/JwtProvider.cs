using Application.Contracts.Authentication;
using Application.Core.Abstractions.Authentication;
using Authentication.Options;
using Domain.UserAggregate;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Application.Core.Abstractions.Common;
using Authentication.Abstractions;

namespace Authentication.Providers;
internal sealed class JwtProvider(
    IOptions<JwtOptions> jwtOptions,
    IClaimsProvider claimsProvider,
    IDateTime dateTime) : IJwtProvider
{
    public async Task<TokenResponse> CreateAsync(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecurityKey));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = await claimsProvider.GetClaimsAsync(user);

        var tokenExpirationTime = dateTime.UtcNow.AddMinutes(jwtOptions.Value.TokenExpirationInMinutes);

        var token = new JwtSecurityToken(
            jwtOptions.Value.Issuer,
            jwtOptions.Value.Audience,
            claims,
            null,
            tokenExpirationTime,
            signingCredentials);

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return new TokenResponse(tokenValue);
    }
}
