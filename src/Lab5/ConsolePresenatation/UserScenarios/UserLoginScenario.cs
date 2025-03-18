using Application.Ports;
using Spectre.Console;

namespace ConsolePresenatation.UserScenarios;

public class UserLoginScenario : IScenario
{
    private readonly IAccountService _accountService;
    private readonly UserManagingScenario _accountManagingScenario;

    public UserLoginScenario(IAccountService accountService, UserManagingScenario accountManagingScenario)
    {
        _accountService = accountService;
        _accountManagingScenario = accountManagingScenario;
    }

    public string Name { get; private set; } = "UserScenario";

    public void Run()
    {
        AnsiConsole.MarkupLine("[bold green]Dear customer you have to Login![/]");
        string login = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter your username"));
        string pin = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter your account pin code").Secret());

        if (_accountService.Login(login, pin))
        {
            AnsiConsole.MarkupLine("[bold green]Logged in succesfull!");
            _accountManagingScenario.Run();
        }
        else
        {
            AnsiConsole.Ask<string>("[bold red]Incorrect password or username![/]!");
        }

        AnsiConsole.Clear();
        return;
    }
}