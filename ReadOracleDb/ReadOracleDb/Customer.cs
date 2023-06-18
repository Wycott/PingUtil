using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadOracleDb
{
    internal class Customer
    {
        internal int CustomerId { get; set; }
        internal string CustomerName { get; set; }
        internal string City { get; set; }

        public override string ToString()
        {
            return $"{CustomerId} - {CustomerName} in {City}";
        }
    }
}
