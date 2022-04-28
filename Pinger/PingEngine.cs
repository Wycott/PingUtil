using System.Net.NetworkInformation;
using System.Text;

namespace Pinger
{
    internal static class PingEngine
    {
        internal static void Start()
        {
            ConsoleColor usual = Console.ForegroundColor;

            long shortest = long.MaxValue;
            long longest = long.MinValue;
            long totalPings = 0;
            long successfulPings = 0;
            long failedPings = 0;
            decimal successRate;
            decimal avgTime = 0;
            long totalTime = 0;
            

            string data = "All our lives we sweat and save.";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 10000;
            const int snoozeTime = 3000;
            const string Brizzy = "8.8.8.8";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Host: {Brizzy}, Timeout: {timeout}, Packet Size: {buffer.Length}, Snooze Time: {snoozeTime}");
            Console.ForegroundColor = usual;

            while (true)
            {

                var status = PingHost(Brizzy, timeout, buffer);
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

                successRate = Math.Round(((decimal)successfulPings / (decimal)totalPings) * (decimal)100, 1);

                if (!status.Success)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                if (status.PingTime > avgTime)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine($"{successRate}% R{status.PingTime}. T{totalPings} P{successfulPings} F{failedPings}. A{avgTime} S{shortest} L{longest}");

                Console.ForegroundColor = usual;
                Thread.Sleep(snoozeTime);
            }
        }

        internal static PingStats PingHost(string nameOrAddress, int timeout, byte[] buffer)
        {
            Ping? pinger = null;
            var ps = new PingStats();

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress, timeout, buffer);
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
