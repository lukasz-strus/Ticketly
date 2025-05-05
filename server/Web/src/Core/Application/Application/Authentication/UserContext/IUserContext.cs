namespace Application.Authentication.UserContext;

public interface IUserContext
{
    CurrentUser? GetCurrentUser();
}