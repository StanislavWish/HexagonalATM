using ConsolePresenatation.AdminScenarios;
using ConsolePresenatation.UserScenarios;
using Microsoft.Extensions.DependencyInjection;

namespace ConsolePresenatation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationConsole(this IServiceCollection collection)
    {
        collection.AddScoped<UserLoginScenario>();
        collection.AddScoped<UserManagingScenario>();

        collection.AddScoped<StartingScenario>();
        collection.AddScoped<AdminLoginScenario>();
        collection.AddScoped<AdminManagingScenario>();

        return collection;
    }
}