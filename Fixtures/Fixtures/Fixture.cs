using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixtures
{
    public class Fixture : IFixture
    {
        public int HomeTeam { get; set; }
        public int AwayTeam { get; set; }

        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }

        public string SortOrder { get; set; }

        public bool Played { get; set; }

        public override string ToString()
        {
            return HomeTeam.ToString() + "," + AwayTeam.ToString() + ":\t" + HomeGoals + "-" + AwayGoals;
        }
    }
}
