using System.Diagnostics;
using ReadOracleDb;

var sw = new Stopwatch();
sw.Start();

var custs = OracleReader.DoRead();

foreach (var cust in custs)
{
    Console.WriteLine(cust);
}

Console.WriteLine($"Ran in {sw.ElapsedMilliseconds}ms");

