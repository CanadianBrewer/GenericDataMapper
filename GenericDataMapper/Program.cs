using System.Data;
using System.Diagnostics;
using GenericDataMapper.DataAccess;
using GenericDataMapper.Extensions;
using GenericDataMapper.Models;

int maxRows = 10000;
Console.WriteLine("Spinning up the hamster wheel...");

// load the db
SqLiteDataAccess da = new();
int rowsToReturn = maxRows;
if (args.Length == 0) {
    da.LoadData(maxRows);
}
else {
    rowsToReturn = Convert.ToInt32(args[0]);
}

da = new();
DataTable dt = da.GetData($"SELECT id, first_name, last_name, city, date_of_birth, is_employee FROM Person LIMIT {rowsToReturn}");
Stopwatch sw = Stopwatch.StartNew();
List<Person> persons = dt.ToList<Person>();
sw.Stop();
Console.WriteLine($"Reflection on {rowsToReturn} Person took {sw.Elapsed} ms");

sw = Stopwatch.StartNew();
List<Person> personsClassic = dt.ToPersonList();
sw.Stop();
Console.WriteLine($"Classic approach on {rowsToReturn} Person took {sw.Elapsed} ms");

da = new();
dt = da.GetData($"SELECT id, corporate_name, ticker_symbol, current_stock_price, fiscal_year_end, board_member_1, board_member_2, board_member_3 FROM Company LIMIT {rowsToReturn}");
sw = Stopwatch.StartNew();
List<Company> companies = dt.ToList<Company>(); 
sw.Stop();
Console.WriteLine($"Reflection on {rowsToReturn} Company took {sw.Elapsed} ms");

sw = Stopwatch.StartNew();
List<Company> companiesClassic = dt.ToCompanyList();
sw.Stop();
Console.WriteLine($"Classic approach on {rowsToReturn} Company took {sw.Elapsed} ms");

