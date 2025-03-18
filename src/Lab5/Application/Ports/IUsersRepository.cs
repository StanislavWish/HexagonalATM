using Application.Core.Users;

namespace Application.Ports;

public interface IUsersRepository
{
    User? GetByUsername(string username);

    User ChangeUsersMoney(User account, decimal money);

    IEnumerable<User> GetAllUsers();

    void AddUser(User user);

    void DeleteUser(string accountId, string accountName);
}