using System.Diagnostics.CodeAnalysis;
using AiAnnotations;
using AiAnnotations.Types;
using Pinger.Domain;
using Pinger.Interfaces;
using SimpleInjector;

namespace Pinger;

[AiGenerated(Authorship.Hybrid)]
public static class Program
{
    [ExcludeFromCodeCoverage]
    private static Container? Container { get; set; }

    [ExcludeFromCodeCoverage]
    public static void Main()
    {
        Container = ConfigureContainer();
        var engine = Container.GetInstance<IPingEngine>();
        StartWork(engine);
    }

    private static Container ConfigureContainer()
    {
        var container = new Container();

        container.Register<IPingEngine, PingEngine>();
        container.Register<IPingTools, PingTools>();
        container.Register<IPingDisplay, PingDisplay>();
        container.Register<IConsoleHandler, ConsoleHandler>();
        container.Register<IPingConfig, PingConfig>();
        container.Register<IRollingStatistics, RollingStatistics>();

        container.Verify();

        return container;
    }

    public static void StartWork(IPingEngine engine)
    {
        engine.Start();
    }
}