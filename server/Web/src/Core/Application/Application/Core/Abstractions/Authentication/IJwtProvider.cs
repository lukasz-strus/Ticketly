using Application.Contracts.Authentication;

namespace Application.Core.Abstractions.Authentication;

public interface IJwtProvider
{
    Task<TokenResponse> CreateAsync(Domain.UserAggregate.User user);
}
