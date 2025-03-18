using Application.Core.Operations;
using Application.Core.Users;
using Application.Ports;

namespace Application.Infrastructure.Services;

public class AdminService : IAdminService
{
    private readonly IUsersRepository _usersRepositor;
    private readonly IOperationRepository _operationsRepositor;
    private bool _isAdminPasswordLogged;

    public AdminService(IUsersRepository usersRepositor, IOperationRepository operationsRepositor)
    {
        _usersRepositor = usersRepositor;
        _operationsRepositor = operationsRepositor;
        _isAdminPasswordLogged = false;
    }

    public AdminResult Login(string password)
    {
        if (password == Environment.GetEnvironmentVariable("adminPassword"))
        {
            _isAdminPasswordLogged = true;
            return new AdminResult.Success();
        }

        return new AdminResult.Failure("Incorrect admin password");
    }

    public AdminResult CreateUser(User newUser)
    {
        try
        {
            if (_isAdminPasswordLogged)
            {
                _usersRepositor.AddUser(newUser);
                _operationsRepositor.AddOperation(new OperationInfo(
                    Guid.NewGuid().GetHashCode(),
                    newUser.Id,
                    0,
                    OperationType.UserCreation));
                return new AdminResult.Success();
            }

            return new AdminResult.Failure("Admin is not logged");
        }
        catch (Exception e)
        {
            return new AdminResult.Failure(e.Message);
        }
    }

    public AdminResult DeleteUser(string userId, string userName)
    {
        try
        {
            if (_isAdminPasswordLogged)
            {
                _usersRepositor.DeleteUser(userId, userName);
                _operationsRepositor.AddOperation(new OperationInfo(
                    Guid.NewGuid().GetHashCode(),
                    Convert.ToInt32(userId),
                    0,
                    OperationType.UserDeletion));
                return new AdminResult.Success();
            }

            return new AdminResult.Failure("Admin is not logged");
        }
        catch (Exception e)
        {
            return new AdminResult.Failure(e.Message);
        }
    }
}