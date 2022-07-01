using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;

namespace GenericDataMapper.DataAccess;

public class SqlDataAccess {
    private const string _connectionString = "Server=ROCINANTE;Database=sandbox;Trusted_Connection=True;Encrypt=False;";

    public DataTable GetData(string query) {
        DataTable entries = new();
        using var connection = new SqlConnection(_connectionString);
        var selectCommand = new SqlCommand(query, connection);
        try {
            connection.Open();
            var reader = selectCommand.ExecuteReader();

            if (reader.HasRows) {
                for (var i = 0; i < reader.FieldCount; i++) {
                    entries.Columns.Add(new DataColumn(reader.GetName(i), reader.GetFieldType(i)));
                }
            }

            while (reader.Read()) {
                var row = entries.NewRow();
                for (var i = 0; i < reader.FieldCount; i++) {
                    row[i] = reader.GetValue(i);
                }

                entries.Rows.Add(row);
            }

            connection.Close();
        }
        catch (SqlException ex) {
            Console.WriteLine(ex);
            connection.Close();
            throw;
        }

        return entries;
    }

    public void LoadData(int numberOfRows) {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        for (var i = 1; i <= numberOfRows; i++) {
            var fn = GetRandomString();
            var ln = GetRandomString();
            var c = GetRandomStringOrNull();
            var dob = GetRandomDateOrNull();
            var randomBool = GetRandomBooleanOrNull();
            int? isEmployee = randomBool.HasValue ? randomBool.Value ? 1 : 0 : null;
            var sql =
                $"INSERT INTO Person (first_name, last_name, city, date_of_birth, is_employee) VALUES ('{fn}', '{ln}', '{c}'";
            sql = dob == null ? sql += ", NULL" : sql += $", '{dob}'";
            sql = isEmployee == null ? sql += ", NULL);" : sql += $", {isEmployee});";
            var insertCommand = new SqlCommand(sql, connection);
            _ = insertCommand.ExecuteNonQuery();

            if (i % 1000 == 0) {
                Console.WriteLine($"Person {i}");
            }
        }

        for (var i = 1; i <= numberOfRows; i++) {
            var cn = GetRandomString();
            var ts = GetRandomString(4);
            var sp = GetRandomDoubleOrNull();
            var ye = GetRandomDateOrNull();
            var b1 = GetRandomStringOrNull();
            var b2 = GetRandomStringOrNull();
            var b3 = GetRandomStringOrNull();
            var sql =
                $"INSERT INTO Company (corporate_name, ticker_symbol, current_stock_price, fiscal_year_end, board_member_1, board_member_2, board_member_3) VALUES ('{cn}', '{ts}', priceParam, yearEndParam, '{b1}', '{b2}', '{b3}')";
            sql = sp == null ? sql.Replace("priceParam", "NULL") : sql.Replace("priceParam", sp.ToString());
            sql = ye == null ? sql.Replace("yearEndParam", "NULL") : sql.Replace("yearEndParam", $"'{ye.ToString()}'");
            var insertCommand = new SqlCommand(sql, connection);
            _ = insertCommand.ExecuteNonQuery();

            if (i % 1000 == 0) {
                Console.WriteLine($"Company {i}");
            }
        }
    }

    public void FlushData() {
        using var connection = new SqlConnection(_connectionString);
        var deleteCommand = new SqlCommand("TRUNCATE TABLE Company", connection);
        connection.Open();
        _ = deleteCommand.ExecuteNonQuery();
        deleteCommand = new SqlCommand("TRUNCATE TABLE Person", connection);
        _ = deleteCommand.ExecuteNonQuery();
        connection.Close();
    }

    private string GetRandomString(int length = 15) {
        StringBuilder sb = new();
        Random r = new();
        for (var i = 0; i < length; i++) {
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

        for (var i = 0; i < 15; i++) {
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

        var year = r.Next(1900, 2000);
        var month = r.Next(1, 12);
        var day = r.Next(1, 28);
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