using Domain.UserAggregate;
using System.Security.Claims;

namespace Authentication.Abstractions;
internal interface IClaimsProvider
{
    Task<Claim[]> GetClaimsAsync(User user);
}
