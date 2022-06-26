using System.ComponentModel;

namespace GenericDataMapper.Models;

public class Person
{
    [Description("id")]
    public int Id { get; set; }

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