namespace Application.Core.Users;

public interface IUserAccount
{
    UserResult GetBalance(string userName);

    UserResult ChangeBalance(string userName);
}