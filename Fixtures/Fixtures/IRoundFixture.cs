using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixtures
{
    public interface IRoundFixture
    {
        int RoundNumber { get; set; }
        List<Fixture> FixtureList { get; set; }
    }
}
