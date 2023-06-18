//using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace ReadOracleDb
{
    internal class OracleReader
    {
        internal static IEnumerable<Customer> DoRead()
        {
            const string connectionString = "Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = localhost)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = orclpdb)));User ID=c##dave;Password=gahan;Connection Timeout=600";

            using var oracleConnection = new OracleConnection(connectionString);

            var readCommand = new OracleCommand
            {
                CommandText = "select * from customers",
                Connection = oracleConnection
            };

            oracleConnection.Open();

            var reader = readCommand.ExecuteReader();

            while (reader.Read())
            {
                var customer = new Customer
                {
                    CustomerId = reader.GetInt32(0),
                    CustomerName = reader.GetString(1),
                    City = reader.GetString(2)
                };

                yield return customer;
            }
        }
    }
}
