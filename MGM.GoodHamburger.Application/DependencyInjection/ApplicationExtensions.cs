using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MGM.GoodHamburger.Application.DependencyInjection;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Add MediatR - registra todos os handlers automaticamente
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}
