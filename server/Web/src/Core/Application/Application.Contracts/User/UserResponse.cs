namespace Application.Contracts.User;

public sealed record UserResponse(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    string AddressStreet,
    string AddressBuilding,
    string? AddressRoom,
    string AddressCode,
    string AddressPost,
    string RoleName,
    int RoleId);