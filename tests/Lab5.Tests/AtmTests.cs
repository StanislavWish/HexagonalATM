using Application.Core.Operations;
using Application.Core.Users;
using Application.Infrastructure.Services;
using Application.Ports;
using System.Collections.ObjectModel;
using Xunit;

namespace Lab5.Tests;

public class AtmTests
{
    [Fact]
    public void AtmDepositTest()
    {
        var userService = new AccountService(new MockUserRepository(), new MockOperationsRepository());
        userService.Login("VoiceInTheHead", "0451");
        userService.Deposit(Convert.ToDecimal(100));

        Assert.True(userService.GetBalance() == Convert.ToDecimal(100));
    }

    [Fact]
    public void WithdrawInsufficientMoney()
    {
        var userService = new AccountService(new MockUserRepository(), new MockOperationsRepository());
        userService.Login("VoiceInTheHead", "0451");
        userService.Withdraw(Convert.ToDecimal(200));

        Assert.True(userService.GetBalance() == Convert.ToDecimal(0));
    }

    [Fact]
    public void NormalWithdraw()
    {
        var userService = new AccountService(new MockUserRepository(), new MockOperationsRepository());
        userService.Login("VoiceInTheHead", "0451");
        userService.Deposit(Convert.ToDecimal(100));
        userService.Withdraw(Convert.ToDecimal(50));

        Assert.True(userService.GetBalance() == Convert.ToDecimal(50));
    }

    private class MockUserRepository : IUsersRepository
    {
        // Base balance is 0
        private User _user = new User(1, "VoiceInTheHead", "0451", Convert.ToDecimal(0));

        public User? GetByUsername(string username)
        {
            return _user;
        }

        public User ChangeUsersMoney(User account, decimal money)
        {
            _user = new User(1, "VoiceInTheHead", "0451", money);
            return _user;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return [new User(1, "VoiceInTheHead", "0451", Convert.ToDecimal(0))];
        }

        public void AddUser(User user) { }

        public void DeleteUser(string accountId, string accountName) { }
    }

    private class MockOperationsRepository : IOperationRepository
    {
        public Collection<OperationInfo> GetByUser(User account)
        {
            return new Collection<OperationInfo>();
        }

        public IEnumerable<OperationInfo> GetAllOperations()
        {
            return new List<OperationInfo>();
        }

        public void AddOperation(OperationInfo operation) { }
    }
}