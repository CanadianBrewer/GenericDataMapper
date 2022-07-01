using System.ComponentModel;
using System.Data;
using GenericDataMapper.Models;

namespace GenericDataMapper.Extensions;

public static class DataTableExtensions {
    public static List<T> ToList<T>(this DataTable source) where T : PropertyAccessor, new() {
        List<T> result = new();

        Dictionary<string, string> propertyFieldMappings = new();
        // load these once - no need to get them on each iteration
        foreach (var property in typeof(T).GetProperties()) {
            var attribute =
                property.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as
                    DescriptionAttribute;
            if (string.IsNullOrWhiteSpace(attribute?.Description)) {
                continue;
            }

            propertyFieldMappings.Add(attribute.Description, property.Name);
        }

        foreach (var row in source.Rows.Cast<DataRow>()) {
            var item = new T();
            foreach (var kvp in propertyFieldMappings) {
                var value = row[kvp.Key];
                if (value == DBNull.Value) {
                    continue;
                }

                item[kvp.Value] = value;
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
                StockPrice = ToNullableDecimal(row["current_stock_price"]),
                YearEnd = ToNullableDateTime(row["fiscal_year_end"]),
                BoardMember1 = row["board_member_1"] as string,
                BoardMember2 = row["board_member_2"] as string,
                BoardMember3 = row["board_member_3"] as string
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

        return Convert.ToBoolean(value);
    }

    public static decimal? ToNullableDecimal(object value) {
        return value == DBNull.Value ? null : Convert.ToDecimal(value);
    }
}