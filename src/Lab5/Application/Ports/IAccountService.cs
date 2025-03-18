using Application.Core.Users;

namespace Application.Ports;

public interface IAccountService
{
    User? CurrentUser { get; }

    bool Login(string username, string pin);

    UserResult Withdraw(decimal moneyValue);

    UserResult Deposit(decimal moneyValue);

    decimal GetBalance();
}