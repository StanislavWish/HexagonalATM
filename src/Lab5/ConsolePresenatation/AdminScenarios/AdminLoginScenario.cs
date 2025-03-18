using Application.Core.Users;
using Application.Ports;
using Spectre.Console;

namespace ConsolePresenatation.AdminScenarios;

public class AdminLoginScenario : IScenario
{
    private readonly IAdminService _adminService;
    private readonly AdminManagingScenario _adminManagingScenario;

    public AdminLoginScenario(AdminManagingScenario adminManagingScenario, IAdminService adminService)
    {
        _adminManagingScenario = adminManagingScenario;
        _adminService = adminService;
    }

    public string Name { get; private set; } = "UserScenario";

    public void Run()
    {
        AnsiConsole.MarkupLine("[bold green]Dear admin you have to Login![/]");
        string pin = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter your secret-Admin password").Secret());

        if (_adminService.Login(pin) is AdminResult.Success)
        {
            AnsiConsole.MarkupLine("[bold green]Logged in succesfull, capitan![/]!");
            _adminManagingScenario.Run();
        }
        else
        {
            AnsiConsole.Ask<string>("[bold red]Incorrect password![/]!");
        }
    }
}