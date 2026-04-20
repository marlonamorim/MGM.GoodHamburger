using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MGM.GoodHamburger.Application.DependencyInjection;
using MGM.GoodHamburger.Domain.Repositories;
using MGM.GoodHamburger.Infrastructure.Data;
using MGM.GoodHamburger.Infrastructure.Repositories;
using Testcontainers.PostgreSql;

namespace MGM.GoodHamburger.IntegrationTests.Infrastructure;

/// <summary>
/// Classe base para testes de integração que cria um container PostgreSQL isolado para cada teste.
/// Cada execução de teste terá seu próprio banco de dados completamente novo e independente.
/// </summary>
public abstract class IntegrationTestBase : IAsyncLifetime
{
    private PostgreSqlContainer _postgresContainer = null!;
    private ServiceProvider _serviceProvider = null!;

    protected AppDbContext DbContext { get; private set; } = null!;
    protected IMediator Mediator { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        // Cria um novo container PostgreSQL para este teste
        // Cada teste terá seu próprio container isolado com um nome único
        _postgresContainer = new PostgreSqlBuilder("postgres:16-alpine")
            .WithDatabase("goodhamburger_test")
            .WithUsername("postgres")
            .WithPassword("postgres123")
            .WithCleanUp(true) // Garante limpeza automática
            .Build();

        // Inicia o container PostgreSQL
        await _postgresContainer.StartAsync();

        // Configura os serviços para este teste específico
        var services = new ServiceCollection();

        // Adiciona DbContext com a connection string do container isolado
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(_postgresContainer.GetConnectionString());
            options.EnableSensitiveDataLogging(); // Para debugging nos testes
        });

        // Adiciona os serviços da aplicação (MediatR + Handlers)
        services.AddApplication();

        // Adiciona os repositórios
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IMenuItemRepository, MenuItemRepository>();

        _serviceProvider = services.BuildServiceProvider();
        DbContext = _serviceProvider.GetRequiredService<AppDbContext>();
        Mediator = _serviceProvider.GetRequiredService<IMediator>();

        // Cria o banco de dados e aplica as migrations
        await DbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        // Limpa o DbContext
        await DbContext.DisposeAsync();

        // Limpa o ServiceProvider
        await _serviceProvider.DisposeAsync();

        // Para e destrói o container PostgreSQL
        await _postgresContainer.StopAsync();
        await _postgresContainer.DisposeAsync();
    }
}
