using Application.Extensions;
using ConsolePresenatation;
using ConsolePresenatation.Extensions;
using Microsoft.Extensions.DependencyInjection;
using PostgresInfrastructure.Extensions;

var collection = new ServiceCollection();

collection
    .AddApplication()
    .AddInfrastructureDataAccess(configuration =>
    {
        configuration.Host = "localhost";
        configuration.Port = 6432;
        configuration.Username = "postgres";
        configuration.Password = "postgres";
        configuration.Database = "postgres";
    }).AddPresentationConsole();

ServiceProvider provider = collection.BuildServiceProvider();
using IServiceScope scope = provider.CreateScope();

scope.UseInfrastructureDataAccess();

IServiceProvider serviceProvider = scope.ServiceProvider;

StartingScenario adminService = serviceProvider.GetRequiredService<StartingScenario>();
while (true)
{
    adminService.Run();
}