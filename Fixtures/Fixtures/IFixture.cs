using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixtures
{
    public interface IFixture
    {
        int HomeTeam { get; set; }
        int AwayTeam { get; set; }

        int HomeGoals { get; set; }
        int AwayGoals { get; set; }

        string SortOrder { get; set; }

        bool Played { get; set; }

    }
}
