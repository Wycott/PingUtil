using System.Threading.Channels;

namespace BuildFootballScore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var allhome = 0;
            var allaway = 0;
            var scores = 25;
            while (scores > 0)
            {
                var lhs = FetchScore(true);
                var rhs = FetchScore(false);
                Console.WriteLine($"{lhs}-{rhs}");
                scores--;
                allhome += lhs;
                allaway += rhs;

            }
            Console.WriteLine();
            Console.WriteLine($"{allhome}-{allaway}");
        }

        static int FetchScore(bool homeTeam)
        {
            var topEnd = homeTeam == true ? 5 : 4;
            var goals = 0;
            int chance;

            while (true)
            {
                var random = new Random();
                chance = random.Next(1, 10);

                if (chance <= topEnd)
                {
                    goals++;
                    if (goals == 10)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return goals;
        }
    }
}