using Domain.Core.Primitives;

namespace Domain;

public static class Errors
{
    public static class Authentication
    {
        public static Error InvalidEmailOrPassword => new(
            "Authentication.InvalidEmailOrPassword",
            "The provided email or password combination is invalid.");

        public static Error PasswordsDoNotMatch => new(
            "Authentication.PasswordsDoNotMatch",
            "The password and confirmation password do not match.");

        public static Error PasswordIsNotStrongEnough => new(
            "Authentication.PasswordIsNotStrongEnough",
            "The password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number and one special character.");

        public static Error DuplicateEmail => new(
            "Authentication.DuplicateEmail",
            "The email is already taken.");

        public static Error MailIsRequired => new(
            "Authentication.MailIsRequired",
            "The mail is required.");

        public static Error PasswordIsRequired => new(
            "Authentication.PasswordIsRequired",
            "The password is required.");
    }

    public static class General
    {
        public static Error BadRequest => new(
            "General.BadRequest",
            "The server could not process the request.");

        public static Error EntityNotFound => new(
            "General.EntityNotFound",
            "The entity with the specified identifier was not found.");

        public static Error ServerError => new(
            "General.ServerError",
            "The server encountered an unrecoverable error.");
    }

    public static class ValueObject
    {
        public static Error IdIsRequired = new(
            "ValueObject.IdIsRequired",
            "The id is required.");

        public static Error FirstNameIsRequired = new(
            "ValueObject.FirstNameIsRequired",
            "The first name is required.");

        public static Error FirstNameIsTooLong = new(
            "ValueObject.FirstNameIsTooLong",
            "The first name is too long.");

        public static Error LastNameIsRequired = new(
            "ValueObject.LastNameIsRequired",
            "The last name is required.");

        public static Error LastNameIsTooLong = new(
            "ValueObject.LastNameIsTooLong",
            "The last name is too long.");

        public static Error StreetIsRequired = new(
            "ValueObject.StreetIsRequired",
            "The street is required.");

        public static Error StreetIsTooLong = new(
            "ValueObject.StreetIsTooLong",
            "The street is too long.");

        public static Error BuildingIsRequired = new(
            "ValueObject.BuildingIsRequired",
            "The building is required.");

        public static Error BuildingIsTooLong = new(
            "ValueObject.BuildingIsTooLong",
            "The building is too long.");

        public static Error RoomIsTooLong = new(
            "ValueObject.RoomIsTooLong",
            "The room is too long.");

        public static Error CodeIsRequired = new(
            "ValueObject.CodeIsRequired",
            "The code is required.");

        public static Error CodeIsInvalid = new(
            "ValueObject.CodeIsInvalid",
            "The code is invalid.");

        public static Error PostIsRequired = new(
            "ValueObject.PostIsRequired",
            "The post is required.");

        public static Error PostIsTooLong = new(
            "ValueObject.PostIsTooLong",
            "The post is too long.");

        public static Error DescriptionIsRequired = new(
            "ValueObject.DescriptionIsRequired",
            "The description is required.");

        public static Error NameIsRequired = new(
            "ValueObject.NameIsRequired",
            "The name is required.");

        public static Error NameIsTooLong = new(
            "ValueObject.NameIsTooLong",
            "The name is too long.");

        public static Error AmountValueMustBeGreaterThan0 = new(
            "ValueObject.AmountValueMustBeGreaterThan0",
            "The amount value must be greater than 0.");

        public static Error WrongEmailFormat => new(
            "ValueObject.WrongEmailFormat",
            "The email is in the wrong format.");

        public static Error CodeMustBe5CharactersLong => new(
            "ValueObject.CodeMustBe5CharactersLong",
            "The code must be 5 characters long.");

        public static Error CategoryIdIsRequired => new(
            "ValueObject.CategoryIdIsRequired",
            "The category id is required.");

        public static Error DateIsRequired => new(
            "ValueObject.DateIsRequired",
            "The date is required.");

        public static Error AddressIsRequired => new(
            "ValueObject.AddressIsRequired",
            "The address is required.");
    }

    public static class Enum
    {
        public static Error CurrencyNotFound => new(
            "Enum.CurrencyNotFound",
            "The currency was not found.");
    }
}