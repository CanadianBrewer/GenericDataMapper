using System.Data;
using System.Text;
using Microsoft.Data.Sqlite;

namespace GenericDataMapper.DataAccess;

public class SqLiteDataAccess {
    public DataTable GetData(string query) {
        DataTable entries = new();
        using var connnection = new SqliteConnection("Data Source=Database\\scratch.db");
        SqliteCommand selectCommand =
            new SqliteCommand(query, connnection);
        try {
            connnection.Open();
            SqliteDataReader reader = selectCommand.ExecuteReader();

            if (reader.HasRows)
                for (int i = 0; i < reader.FieldCount; i++) {
                    entries.Columns.Add(new DataColumn(reader.GetName(i)));
                }

            int j = 0;
            while (reader.Read()) {
                DataRow row = entries.NewRow();
                for (int i = 0; i < reader.FieldCount; i++) {
                    row[i] = reader.GetValue(i);
                }

                entries.Rows.Add(row);
                j++;
            }

            connnection.Close();
        }
        catch (SqliteException ex) {
            Console.WriteLine(ex);
            connnection.Close();
            throw;
        }

        return entries;
    }

    public void LoadData(int numberOfRows) {
        using var connnection = new SqliteConnection("Data Source=Database\\scratch.db");
        for (int i = 1; i <= numberOfRows; i++) {
            string fn = GetRandomString();
            string ln = GetRandomString();
            string c = GetRandomStringOrNull();
            DateTime? dob = GetRandomDateOrNull();
            bool? isEmp = GetRandomBooleanOrNull();
            var sql =
                $"INSERT INTO Person (first_name, last_name, city, date_of_birth, is_employee) VALUES ('{fn}', '{ln}', '{c}'";
            sql = dob == null ? sql += ", NULL" : sql += $", '{dob}'";
            sql = isEmp == null ? sql += ", NULL);" : sql += $", {isEmp});";
            SqliteCommand insertCommand = new SqliteCommand(sql, connnection);
            connnection.Open();
            _ = insertCommand.ExecuteNonQuery();

            if (i % 1000 == 0) {
                Console.WriteLine($"Person {i}");
            }
        }

        for (int i = 1; i <= numberOfRows; i++) {
            string cn = GetRandomString();
            string ts = GetRandomString(4);
            double? sp = GetRandomDoubleOrNull();
            DateTime? ye = GetRandomDateOrNull();
            string b1 = GetRandomStringOrNull();
            string b2 = GetRandomStringOrNull();
            string b3 = GetRandomStringOrNull();
            var sql = $"INSERT INTO Company (corporate_name, ticker_symbol, current_stock_price, fiscal_year_end, board_member_1, board_member_2, board_member_3) VALUES ('{cn}', '{ts}', priceParam, yearEndParam, '{b1}', '{b2}', '{b3}')";
            sql = sp == null ? sql.Replace("priceParam", "NULL") : sql.Replace("priceParam", sp.ToString());
            sql = ye == null ? sql.Replace("yearEndParam", "NULL") : sql.Replace("yearEndParam", $"'{ye.ToString()}'");
            SqliteCommand insertCommand = new SqliteCommand(sql, connnection);
            _ = insertCommand.ExecuteNonQuery();

            if (i % 1000 == 0) {
                Console.WriteLine($"Company {i}");
            }
        }

        SqliteCommand countCommand = new SqliteCommand("SELECT COUNT(*) FROM Person", connnection);
        Console.WriteLine($"person has {countCommand.ExecuteScalar()} rows");
        countCommand = new SqliteCommand("SELECT COUNT(*) FROM Company", connnection);
        Console.WriteLine($"company has {countCommand.ExecuteScalar()} rows");
    }

    public void FlushData() {
        using var connnection = new SqliteConnection("Data Source=Database\\scratch.db");
        SqliteCommand deleteCommand = new SqliteCommand("DELETE FROM Company WHERE id > 5", connnection);
        connnection.Open();
        _ = deleteCommand.ExecuteNonQuery();
        deleteCommand = new SqliteCommand("DELETE FROM Person WHERE id > 5", connnection);
        _ = deleteCommand.ExecuteNonQuery();
        connnection.Close();
    }

    private string GetRandomString(int length = 15) {
        StringBuilder sb = new();
        Random r = new();
        for (int i = 0; i < length; i++) {
            sb.Append((char)r.Next(65, 90));
        }

        return sb.ToString();
    }

    private string GetRandomStringOrNull() {
        StringBuilder sb = new();
        Random r = new();
        if (r.Next(0, 100) > 75) {
            return null;
        }

        for (int i = 0; i < 15; i++) {
            sb.Append((char)r.Next(65, 90));
        }

        return sb.ToString();
    }

    private double? GetRandomDoubleOrNull() {
        Random r = new();
        if (r.Next(0, 100) > 75) {
            return null;
        }

        return r.NextDouble() * 1000;
    }

    private DateTime? GetRandomDateOrNull() {
        Random r = new();
        if (r.Next(0, 100) > 75) {
            return null;
        }

        int year = r.Next(1900, 2000);
        int month = r.Next(1, 12);
        int day = r.Next(1, 28);
        return new DateTime(year, month, day);
    }
    
    private bool? GetRandomBooleanOrNull() {
        Random r = new();
        if (r.Next(0, 100) > 75) {
            return null;
        }

        return r.NextDouble() > 0.5;
    }
}
