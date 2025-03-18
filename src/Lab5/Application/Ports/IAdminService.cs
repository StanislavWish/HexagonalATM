using Application.Core.Users;

namespace Application.Ports;

public interface IAdminService
{
    AdminResult Login(string password);

    AdminResult CreateUser(User newUser);

    AdminResult DeleteUser(string userId, string userName);
}