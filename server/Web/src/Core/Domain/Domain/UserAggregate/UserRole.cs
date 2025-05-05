using Domain.Core.Primitives;

namespace Domain.UserAggregate;

public sealed class UserRole : Enumeration<UserRole>
{
    public static readonly UserRole Admin = new(1, UserRoleNames.Admin);
    public static readonly UserRole User = new(2, UserRoleNames.User);

    private UserRole(
        int value,
        string name) : base(value, name)
    {
    }

    // Ctor for EF
    // ReSharper disable once UnusedMember.Local
    private UserRole()
    {
    }
}

public static class UserRoleNames
{
    public const string Admin = "Admin";
    public const string User = "User";
}