using System.ComponentModel;

namespace GenericDataMapper.Models;

public class Person : PropertyAccessor {
    [Description("id")]
    public long Id { get; set; }

    [Description("first_name")]
    public string FirstName { get; set; }

    [Description("last_name")]
    public string LastName { get; set; }

    [Description("city")]
    public string? Address { get; set; }

    [Description("date_of_birth")]
    public DateTime? DateOfBirth { get; set; }

    [Description("is_employee")]
    public bool? IsEmployee { get; set; }
}