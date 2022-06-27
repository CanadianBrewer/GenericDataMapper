using System.ComponentModel;
using System.Reflection;
using System.Data;
using GenericDataMapper.Models;

namespace GenericDataMapper.Extensions;

public static class DataTableExtensions {
    public static List<T> ToList<T>(this DataTable source) where T : new() {
        List<T> result = new();
        foreach (var row in source.Rows.Cast<DataRow>()) {
            var item = new T();
            foreach (var property in typeof(T).GetProperties()) {
                DescriptionAttribute attribute =
                    property.GetCustomAttributes(typeof(DescriptionAttribute), false).First() as DescriptionAttribute;
                if (string.IsNullOrWhiteSpace(attribute?.Description)) {
                    continue;
                }

                object value = row[attribute.Description];
                if (value == DBNull.Value) {
                    continue;
                }

                Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                
                // SQLite has a BOOLEAN type but it's really a wrapper over an integer.
                // Without this, the value is "0" or "1" (yep, a string) and Convert.ChangeType blows up.
                // Should see how SQL Server data types come back in future iterations.
                if (t == typeof(bool)) {
                    value = Convert.ToInt32(value);
                }

                object safeValue = Convert.ChangeType(value, t);
                property.SetValue(item, safeValue, null);
            }

            result.Add(item);
        }

        return result;
    }

    public static List<Person> ToPersonList(this DataTable source) {
        List<Person> persons = new();
        foreach (var row in source.Rows.Cast<DataRow>()) {
            persons.Add(new Person {
                Id = Convert.ToInt32(row["Id"]),
                FirstName = row["first_name"] as string,
                LastName = row["last_name"] as string,
                Address = row["city"] as string,
                DateOfBirth = ToNullableDateTime(row["date_of_birth"]),
                IsEmployee = ToNullableBool(row["is_employee"])
            });
        }

        return persons;
    }

    public static List<Company> ToCompanyList(this DataTable source) {
        List<Company> companies = new();
        foreach (var row in source.Rows.Cast<DataRow>()) {
            companies.Add(new Company {
                Id = Convert.ToInt32(row["Id"]),
                CompanyName = row["corporate_name"] as string,
                TickerSymbol = row["ticker_symbol"] as string,
                StockPrice = ToNullableDouble(row["current_stock_price"]),
                YearEnd = ToNullableDateTime(row["fiscal_year_end"]),
                BoardMember1 = row["board_member_1"] as string,
                BoardMember2 = row["board_member_2"] as string,
                BoardMember3 = row["board_member_3"] as string,
            });
        }

        return companies;
    }

    public static DateTime? ToNullableDateTime(object value) {
        return value == DBNull.Value ? null : Convert.ToDateTime(value);
    }

    public static bool? ToNullableBool(object value) {
        if (value == DBNull.Value) {
            return null;
            
        }

        return value.ToString() == "1";
    }

    public static double? ToNullableDouble(object value) {
        return value == DBNull.Value ? null : Convert.ToDouble(value);
    }
}