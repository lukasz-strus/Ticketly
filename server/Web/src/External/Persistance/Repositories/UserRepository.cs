using Domain.UserAggregate;
using Microsoft.AspNetCore.Identity;

namespace Persistance.Repositories;

internal sealed class UserRepository(
    UserManager<User> userManager) : IUserRepository
{
    public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default) =>
        await userManager.FindByIdAsync(id);

    public async Task<UserRole> GetUserRole(string id, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(id, cancellationToken);
        if (user == null)
            throw new Exception("User not found");

        var rolesIList = await userManager.GetRolesAsync(user);
        var roles = rolesIList.ToList();

        if (roles.Count == 2)
            return UserRole.Admin;

        if (roles.Count == 0 || roles.FirstOrDefault() is not { } roleName)
            throw new Exception("User has no roles");

        var role = UserRole.FromName(roleName);
        if (role == null)
            throw new Exception("User has no roles");

        return role;
    }

    public async Task Insert(User user, string password)
    {
        var result = await userManager.CreateAsync(
            user,
            password);

        if (!result.Succeeded)
            throw new Exception(result.Errors.First().Description);

        await userManager.AddToRoleAsync(user, UserRole.User.Name);
    }

    public async Task<bool> IsEmailUniqueAsync(string? email) =>
        email is not null && await userManager.FindByEmailAsync(email) is null;
}