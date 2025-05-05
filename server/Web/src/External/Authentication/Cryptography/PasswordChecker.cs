using Domain.UserAggregate;
using Domain.UserAggregate.Services;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Cryptography;

internal sealed class PasswordChecker(
    SignInManager<User> signInManager) : IPasswordChecker
{
    public bool Check(User user, string password)
    {
        var result = signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);

        return result.Result.Succeeded;
    }
}
