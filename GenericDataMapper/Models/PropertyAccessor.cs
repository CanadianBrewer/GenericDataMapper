namespace GenericDataMapper.Models;

/// <summary>
///     Magical reflection base class that when added to data objects allows for direct property notation such as
///     object["property"] for both getting and setting values.
/// </summary>
public class PropertyAccessor {
    public object this[string propertyName] {
        get => GetType().GetProperty(propertyName)?.GetValue(this, null);
        set => GetType().GetProperty(propertyName)?.SetValue(this, value, null);
    }
}