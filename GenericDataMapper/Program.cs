using System.Diagnostics;
using GenericDataMapper.DataAccess;
using GenericDataMapper.Extensions;
using GenericDataMapper.Models;

var maxRows = 100000;
Console.WriteLine("Spinning up the hamster wheel...");

// load the db
SqlDataAccess da = new();
var rowsToReturn = maxRows;
if (args.Length == 0) {
    da.LoadData(maxRows);
}
else {
    rowsToReturn = Convert.ToInt32(args[0]);
}

if (rowsToReturn == -1) {
    da.FlushData();
    Console.WriteLine("Flushing the data");
    return;
}

da = new SqlDataAccess();
var dt = da.GetData(
    $"SELECT TOP {rowsToReturn} id, first_name, last_name, city, date_of_birth, is_employee FROM Person");
long totalElapsedTicks = 0;

// run it once to incur any "startup cost" which occurs the first time
var persons = dt.ToList<Person>();
for (var i = 0; i < 10; i++) {
    var sw = Stopwatch.StartNew();
    persons = dt.ToList<Person>();
    sw.Stop();
    totalElapsedTicks += sw.ElapsedTicks;
}

var ts = new TimeSpan(totalElapsedTicks);
Console.WriteLine($"10 iterations of Reflection on {rowsToReturn} Person averaged {(ts.TotalMilliseconds/10):###.00000} ms");


// run it once to incur any "startup cost" which occurs the first time
var personsClassic = dt.ToPersonList();
totalElapsedTicks = 0;
for (var i = 0; i < 10; i++) {
    var sw = Stopwatch.StartNew();
    personsClassic = dt.ToPersonList();
    sw.Stop();
    totalElapsedTicks += sw.ElapsedTicks;
}

ts = new TimeSpan(totalElapsedTicks);
Console.WriteLine($"10 iterations of Classic on {rowsToReturn} Person averaged {(ts.TotalMilliseconds/10):###.00000} ms");


da = new SqlDataAccess();
dt = da.GetData(
    $"SELECT TOP {rowsToReturn} id, corporate_name, ticker_symbol, current_stock_price, fiscal_year_end, board_member_1, board_member_2, board_member_3 FROM Company ");
totalElapsedTicks = 0;

// run it once to incur any "startup cost" which occurs the first time
var companies = dt.ToList<Company>();
for (var i = 0; i < 10; i++) {
    var sw = Stopwatch.StartNew();
    companies = dt.ToList<Company>();
    sw.Stop();
    totalElapsedTicks += sw.ElapsedTicks;
}

ts = new TimeSpan(totalElapsedTicks);
Console.WriteLine($"10 iterations of Reflection on {rowsToReturn} Company averaged {(ts.TotalMilliseconds/10):###.00000} ms");


totalElapsedTicks = 0;
// run it once to incur any "startup cost" which occurs the first time
var companiesClassic = dt.ToCompanyList();
for (var i = 0; i < 10; i++) {
    var sw = Stopwatch.StartNew();
    companiesClassic = dt.ToCompanyList();
    sw.Stop();
    totalElapsedTicks += sw.ElapsedTicks;
}

ts = new TimeSpan(totalElapsedTicks);
Console.WriteLine($"10 iterations of Classic approach on {rowsToReturn} Company averaged {(ts.TotalMilliseconds/10):###.00000} ms");

Console.WriteLine("And the hamster takes a nap.");