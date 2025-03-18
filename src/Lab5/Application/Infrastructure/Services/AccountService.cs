using Application.Core.Operations;
using Application.Core.Users;
using Application.Ports;

namespace Application.Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly IUsersRepository _usersRepositor;
    private readonly IOperationRepository _operationsRepositor;

    public AccountService(IUsersRepository usersRepositor, IOperationRepository operationsRepositor)
    {
        _usersRepositor = usersRepositor;
        _operationsRepositor = operationsRepositor;
    }

    public User? CurrentUser { get; private set; }

    public bool Login(string username, string pin)
    {
        User? existingUser = _usersRepositor.GetByUsername(username);
        if (existingUser != null && existingUser.PinCode == pin)
        {
            CurrentUser = existingUser;
            return true;
        }

        return false;
    }

    public UserResult Withdraw(decimal moneyValue)
    {
        if (CurrentUser == null)
            return new UserResult.Failure("You have to login first.");
        if (GetBalance() - moneyValue >= 0)
        {
            _usersRepositor.ChangeUsersMoney(
                CurrentUser,
                Convert.ToDecimal(GetBalance() - moneyValue));
            _operationsRepositor.AddOperation(new OperationInfo(
                Guid.NewGuid().GetHashCode(),
                CurrentUser.Id,
                moneyValue,
                OperationType.Withdraw));

            return new UserResult.Success(moneyValue);
        }
        else
        {
            return new UserResult.Failure("You don't have enough money.");
        }
    }

    public UserResult Deposit(decimal moneyValue)
    {
        if (CurrentUser == null)
            return new UserResult.Failure("You have to login first.");

        _usersRepositor.ChangeUsersMoney(
            CurrentUser,
            Convert.ToDecimal(CurrentUser.Balance) + moneyValue);
        _operationsRepositor.AddOperation(new OperationInfo(
            Guid.NewGuid().GetHashCode(),
            CurrentUser.Id,
            moneyValue,
            OperationType.Deposit));

        return new UserResult.Success(moneyValue);
    }

    public decimal GetBalance()
    {
        if (CurrentUser is not null)
            return Convert.ToDecimal(_usersRepositor.GetByUsername(CurrentUser.Name)?.Balance);

        return 0;
    }
}