using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixtures
{
    public class RoundFixtures : IRoundFixture
    {
        public int RoundNumber { get; set; }
        public List<Fixture> FixtureList { get; set; }
    }
}
