using ConsolePresenatation.AdminScenarios;
using ConsolePresenatation.UserScenarios;
using Spectre.Console;

namespace ConsolePresenatation;

public class StartingScenario : IScenario
{
    private readonly AdminLoginScenario _adminLoginScenario;
    private readonly UserLoginScenario _userLoginScenario;

    public StartingScenario(UserLoginScenario userLoginScenario, AdminLoginScenario adminLoginScenario)
    {
        _userLoginScenario = userLoginScenario;
        _adminLoginScenario = adminLoginScenario;
    }

    public string Name { get; private set; } = "StartingScenario";

    public void Run()
    {
        AnsiConsole.MarkupLine("[bold green]Welcome to our fancy ATM System![/]");
        string workingMode = AnsiConsole.Prompt(
            new TextPrompt<string>("Are you our customer or a Admin of ATM?")
                .AddChoices(["Admin", "User"])
                .DefaultValue("User"));

        if (workingMode.Equals("Admin", StringComparison.Ordinal))
        {
            _adminLoginScenario.Run();
        }
        else
        {
            _userLoginScenario.Run();
        }
    }
}