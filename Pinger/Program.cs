using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pinger.Domain;
using Pinger.Interfaces;

namespace Pinger;

public static class Program
{
    [ExcludeFromCodeCoverage]
    public static void Main()
    {
        using var serviceProvider = ConfigureServices();

        var engine = serviceProvider.GetRequiredService<IPingEngine>();

        StartWork(engine);
    }

    public static ServiceProvider ConfigureServices()
    {
        const string configFile = "appsettings.json";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(configFile, optional: true, reloadOnChange: false)
            .Build();

        var services = new ServiceCollection();

        services.AddSingleton<IConfiguration>(configuration);
        services.AddTransient<IPingEngine, PingEngine>();
        services.AddTransient<IPingTools, PingTools>();
        services.AddTransient<IPingDisplay, PingDisplay>();
        services.AddTransient<IConsoleHandler, ConsoleHandler>();
        services.AddSingleton<IPingConfig>(new PingConfig(configuration));
        services.AddSingleton<IRollingStatistics, RollingStatistics>();

        return services.BuildServiceProvider();
    }

    public static void StartWork(IPingEngine engine) => engine.Start();
}
