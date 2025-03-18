using Application.Core.Users;
using Application.Ports;
using Spectre.Console;

namespace ConsolePresenatation.UserScenarios;

public class UserManagingScenario : IScenario
{
    public string Name { get; } = "User Managment";

    private readonly IAccountService _accountService;

    public UserManagingScenario(IAccountService adminService)
    {
        _accountService = adminService;
    }

    public void Run()
    {
        AnsiConsole.MarkupLine("[bold green]Choose an action:[/]");
        string action = AnsiConsole.Prompt(
            new TextPrompt<string>("What you want to do?")
                .AddChoices(["Withdraw", "Deposit", "Check Balance", "Leave"]));

        switch (action)
        {
            case "Withdraw":
                string withdrawAmount = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter the amount of money you want to withdraw"));
                UserResult withdrawResult = _accountService.Withdraw(Convert.ToDecimal(withdrawAmount));
                if (withdrawResult is UserResult.Success)
                {
                    AnsiConsole.MarkupLine("[green]Success![/]");
                }

                if (withdrawResult is UserResult.Failure withdrawFailure)
                {
                    AnsiConsole.MarkupLine($"[red]Sorry there is a error: {withdrawFailure.Message}[/]");
                }

                break;

            case "Deposit":
                string depositAmount = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter the amount of money you want to deposit"));
                UserResult depositResult = _accountService.Deposit(Convert.ToDecimal(depositAmount));
                if (depositResult is UserResult.Success)
                {
                    AnsiConsole.MarkupLine("[green]Success![/]");
                }

                if (depositResult is UserResult.Failure depositFailure)
                {
                    AnsiConsole.MarkupLine($"[red] how did we get here [/] Sorry there is a error: {depositFailure.Message}[/]");
                }

                break;

            case "Leave":
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[bold white] See you space cowboy. [/]");
                return;
        }
    }
}