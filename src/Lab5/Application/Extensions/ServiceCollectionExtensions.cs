using Application.Infrastructure.Services;
using Application.Ports;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddSingleton<IAccountService, AccountService>();
        collection.AddSingleton<IAdminService, AdminService>();

        return collection;
    }
}