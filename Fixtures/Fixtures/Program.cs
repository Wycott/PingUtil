using System;
using System.Collections.Generic;
using Fixtures;

public class Program
{
    public static void Main()
    {
        IFixtureBuilder builder = new FixtureBuilder();
        var roundFixtures = builder.GetRoundFixtures(20);
        DisplayFixtures(roundFixtures);
    }

    private static void DisplayFixtures(List<RoundFixtures> roundFixturesList)
    {
        int roundNumber = 1;
        foreach (var round in roundFixturesList.OrderBy(x => x.RoundNumber))
        {
            Console.WriteLine($"ROUND: {roundNumber}");
            Console.WriteLine();

            foreach (var f in round.FixtureList)
            {
                Console.WriteLine(f);
            }

            Console.WriteLine($"Games in round: {round.FixtureList.Count}");
            Console.WriteLine();

            roundNumber++;
        }
    }


}



