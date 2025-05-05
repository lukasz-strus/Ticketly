using System.ComponentModel.DataAnnotations.Schema;
using Domain.OrderAggregate;
using Domain.UserAggregate.Services;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Domain.UserAggregate;

public class User : IdentityUser
{
    [PersonalData] public required FirstName FirstName { get; set; }
    [PersonalData] public required LastName LastName { get; set; }
    [PersonalData] public required Address Address { get; set; }
    public List<Order> Orders { get; } = [];

    public bool VerifyPassword(string password, IPasswordChecker passwordHashChecker)
        => !string.IsNullOrWhiteSpace(password) && passwordHashChecker.Check(this, password);
}