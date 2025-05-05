namespace Domain.UserAggregate.Services;

public interface IPasswordChecker
{
    bool Check(User user, string password);
}
