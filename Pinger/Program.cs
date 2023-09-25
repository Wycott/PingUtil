using System.Diagnostics.CodeAnalysis;
using Pinger.Interfaces;
using SimpleInjector;

namespace Pinger;

public static class Program
{
    private static Container? Container { get; set; }

    [ExcludeFromCodeCoverage]
    public static void Main()
    {
        Container = new Container();

        Container.Register<IPingEngine, PingEngine>();
        Container.Verify();

        var engine = Container.GetInstance<IPingEngine>();
        StartWork(engine);
    }

    public static void StartWork(IPingEngine engine)
    {
        engine.Start();
    }
}