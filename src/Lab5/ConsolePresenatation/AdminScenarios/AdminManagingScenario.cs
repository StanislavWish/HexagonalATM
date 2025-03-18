using Application.Core.Users;
using Application.Ports;
using Spectre.Console;

namespace ConsolePresenatation.AdminScenarios;

public class AdminManagingScenario : IScenario
{
    public string Name { get; private set; } = "Admin Management";

    private readonly IAdminService _adminService;

    public AdminManagingScenario(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public void Run()
    {
        AnsiConsole.MarkupLine("[bold green]Choose an action:[/]");
        string action = AnsiConsole.Prompt(
            new TextPrompt<string>("What you want to do?")
                .AddChoices(["Create user", "Delete user", "Leave"]));

        switch (action)
        {
            case "Create":
                AnsiConsole.Clear();
                string userName = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter Username of new account"));
                string userPin = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter pin-code of that person"));

                var newAccount = new User(1, userName, userPin, Convert.ToDecimal(0.0));
                AdminResult creationResult = _adminService.CreateUser(newAccount);
                if (creationResult is AdminResult.Success)
                    AnsiConsole.MarkupLine("[bold green]User deleted successfully![/]");

                if (creationResult is AdminResult.Failure fail)
                {
                    AnsiConsole.MarkupLine($"[bold green]Sorry, there is an error: {fail.Message}[/]![/]");
                }

                break;

            case "Delete":
                AnsiConsole.Clear();
                string usersIdToDelete = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter User ID you want to delete"));
                string usersNameToDelete = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter username of that person"));

                AdminResult delitionResult = _adminService.DeleteUser(usersIdToDelete, usersNameToDelete);

                if (delitionResult is AdminResult.Success)
                    AnsiConsole.MarkupLine("[bold green]User deleted successfully![/]");

                if (delitionResult is AdminResult.Failure failure)
                {
                    AnsiConsole.MarkupLine($"[bold green]Sorry, there is an error: {failure.Message}[/]![/]");
                }

                break;

            case "Leave":
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[bold white] See you space cowboy. [/]");
                return;
        }
    }
}