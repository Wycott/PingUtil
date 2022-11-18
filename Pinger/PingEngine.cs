using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;

namespace Pinger
{
    internal static class PingEngine
    {
        internal static void Start()
        {
            var usual = Console.ForegroundColor;

            var shortest = long.MaxValue;
            var longest = long.MinValue;
            long totalPings = 0;
            long successfulPings = 0;
            long failedPings = 0;
            decimal avgTime = 0;
            long totalTime = 0;


            const string Data = "All our lives we sweat and save.";
            var buffer = Encoding.ASCII.GetBytes(Data);
            const int Timeout = 10000;
            const int SnoozeTime = 5000;
            const string RemoteServer = "8.8.8.8";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Host: {RemoteServer}, Timeout: {Timeout}, Packet Size: {buffer.Length}, Snooze Time: {SnoozeTime}");
            Console.ForegroundColor = usual;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {

                var status = PingHost(RemoteServer, Timeout, buffer);
                totalPings++;

                if (status.Success)
                {
                    successfulPings++;
                }
                else
                {
                    failedPings++;
                }

                if (status.Success)
                {
                    totalTime += status.PingTime;
                    avgTime = Math.Round(totalTime / (decimal)successfulPings, 1);

                    if (status.PingTime > longest)
                    {
                        longest = status.PingTime;
                    }

                    if (status.PingTime < shortest)
                    {
                        shortest = status.PingTime;
                    }
                }

                var successRate = Math.Round((successfulPings / (decimal)totalPings) * 100, 1);

                if (!status.Success)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                if (status.PingTime > avgTime)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                var t = TimeSpan.FromSeconds(sw.ElapsedMilliseconds / 1000);
                var hours = t.Hours;
                var minutes = t.Minutes;
                var seconds = t.Seconds;

                var elapsed = $"{hours:00}:{minutes:00}:{seconds:00}";

                Console.WriteLine($"{successRate}% R{status.PingTime}. T{totalPings} P{successfulPings} F{failedPings}. A{avgTime} S{shortest} L{longest} U{elapsed}");

                Console.ForegroundColor = usual;
                Thread.Sleep(SnoozeTime);
            }
        }

        internal static PingStats PingHost(string nameOrAddress, int timeout, byte[] buffer)
        {
            Ping? pinger = null;
            var ps = new PingStats();

            try
            {
                pinger = new Ping();
                var reply = pinger.Send(nameOrAddress, timeout, buffer);
                ps.Success = reply.Status == IPStatus.Success;
                ps.PingTime = reply.RoundtripTime;
            }
            catch (PingException)
            {
                // Don't care what type of failure it is
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return ps;
        }
    }
}
