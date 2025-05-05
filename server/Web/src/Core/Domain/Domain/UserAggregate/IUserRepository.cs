namespace Domain.UserAggregate;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<UserRole> GetUserRole(string id, CancellationToken cancellationToken = default);

    Task Insert(User user, string password);

    Task<bool> IsEmailUniqueAsync(string? email);
}