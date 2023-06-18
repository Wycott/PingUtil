using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixtures
{
    public class FixtureBuilder : IFixtureBuilder
    {
        public List<RoundFixtures> GetRoundFixtures(int numberOfTeams)
        {
            List<Fixture> allFixtures = new List<Fixture>();
            //const int topEnd = 6;

            for (int x = 0; x < numberOfTeams; x++)
            {
                for (int y = 0; y < numberOfTeams; y++)
                {
                    if (x != y)
                    {
                        Fixture f = new Fixture() { HomeTeam = x, AwayTeam = y };
                        if (!allFixtures.Contains(f))
                        {
                            Random random = new Random();
                            f.HomeGoals = random.Next(0, 6); // Random score between 0 and 5 for the home team
                            f.AwayGoals = random.Next(0, 6); // Random score between 0 and 5 for the away team		
                            f.SortOrder = Guid.NewGuid().ToString();
                            allFixtures.Add(f);
                        }
                    }
                }
            }

            int round = 1;

            List<RoundFixtures> roundFixturesList = new List<RoundFixtures>();

            while (allFixtures.Any(x => x.Played == false))
            {
                List<int> teamsPlayed = new List<int>();

                var roundFixtures = new RoundFixtures();
                roundFixtures.RoundNumber = round;
                roundFixtures.FixtureList = new List<Fixture>();

                foreach (var match in allFixtures.Where(x => x.Played == false).OrderBy(x => x.SortOrder))
                {
                    if (!teamsPlayed.Contains(match.HomeTeam) && !teamsPlayed.Contains(match.AwayTeam))
                    {

                        roundFixtures.FixtureList.Add(match);
                        teamsPlayed.Add(match.HomeTeam);
                        teamsPlayed.Add(match.AwayTeam);
                        match.Played = true;
                    }

                }
                roundFixturesList.Add(roundFixtures);
            }

            // Ensure final day of the season has a full set of games

            int maxEntryRound = roundFixturesList.Max(x => x.RoundNumber);

            var finalRound = roundFixturesList.FirstOrDefault(x => x.RoundNumber == maxEntryRound);
            finalRound.RoundNumber = ++maxEntryRound;

            //DisplayFixtures(roundFixturesList);

            return roundFixturesList;
        }

       
    }
}
