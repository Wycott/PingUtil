using System.Diagnostics.CodeAnalysis;
using Pinger.Interfaces;
using SimpleInjector;

namespace Pinger;

public static class Program
{
    [ExcludeFromCodeCoverage]
    private static Container? Container { get; set; }

    [ExcludeFromCodeCoverage]
    public static void Main()
    {
        Container = new Container();

        Container.Register<IPingEngine, PingEngine>();
        Container.Register<IPingTools, PingTools>();
        Container.Register<IPingDisplay, PingDisplay>();
        Container.Register<IConsoleHandler, ConsoleHandler>();
        Container.Register<IPingConfig, PingConfig>();
        Container.Register<IRollingStatistics, RollingStatistics>();
        Container.Verify();

        var engine = Container.GetInstance<IPingEngine>();
        StartWork(engine);
    }

    public static void StartWork(IPingEngine engine)
    {
        engine.Start();
    }
}