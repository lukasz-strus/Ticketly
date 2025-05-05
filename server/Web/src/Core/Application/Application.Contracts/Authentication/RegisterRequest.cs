namespace Application.Contracts.Authentication;

public sealed record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Street,
    string Building,
    string? Room,
    string Code,
    string Post);