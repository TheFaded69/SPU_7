namespace SPU_7.Modbus.Attributes;

/// <summary>
/// Игнорирование сериализации свойства
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class SerializationIgnoredAttribute : Attribute
{
    /// <summary>
    /// Игнорирование
    /// </summary>
    public bool IsIgnored { get; }

    public SerializationIgnoredAttribute()
    {
        IsIgnored = true;
    }
}