using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MGM.GoodHamburger.Domain.Repositories;
using MGM.GoodHamburger.Infrastructure.Data;
using MGM.GoodHamburger.Infrastructure.Repositories;

namespace MGM.GoodHamburger.Infrastructure.DependencyInjection;

public static class RepositoryExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Add Repositories
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IMenuItemRepository, MenuItemRepository>();

        return services;
    }
}
