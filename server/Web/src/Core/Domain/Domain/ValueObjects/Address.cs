using System.ComponentModel.DataAnnotations.Schema;
using Domain.Core.Results;

namespace Domain.ValueObjects;

[ComplexType]
public sealed record Address
{
    private Address(string street, string building, string? room, string code, string post)
    {
        Street = street;
        Building = building;
        Room = room;
        Code = code;
        Post = post;
    }

    public const int StreetMaxLength = 100;
    public const int BuildingMaxLength = 10;
    public const int RoomMaxLength = 10;
    public const int CodeLength = 5;
    public const int PostMaxLength = 50;

    public string Street { get; init; }
    public string Building { get; init; }
    public string? Room { get; init; }
    public string Code { get; init; }
    public string Post { get; init; }

    public static Result<Address> Create(string street, string building, string? room, string code, string post)
    {
        if (string.IsNullOrWhiteSpace(street))
            return Result.Failure<Address>(Errors.ValueObject.StreetIsRequired);

        if (string.IsNullOrWhiteSpace(building))
            return Result.Failure<Address>(Errors.ValueObject.BuildingIsRequired);

        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure<Address>(Errors.ValueObject.CodeIsRequired);

        if (string.IsNullOrWhiteSpace(post))
            return Result.Failure<Address>(Errors.ValueObject.PostIsRequired);

        if (street.Length > StreetMaxLength)
            return Result.Failure<Address>(Errors.ValueObject.StreetIsTooLong);

        if (building.Length > BuildingMaxLength)
            return Result.Failure<Address>(Errors.ValueObject.BuildingIsTooLong);

        if (!string.IsNullOrWhiteSpace(room) && room.Length > RoomMaxLength)
            return Result.Failure<Address>(Errors.ValueObject.RoomIsTooLong);

        var clearCode = code.Replace("-", string.Empty);
        if (clearCode.Length != CodeLength)
            return Result.Failure<Address>(Errors.ValueObject.CodeIsInvalid);

        if (post.Length > PostMaxLength)
            return Result.Failure<Address>(Errors.ValueObject.PostIsTooLong);

        return new Address(street, building, room, clearCode, post);
    }
}