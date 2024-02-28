namespace SPU_7.Modbus.Attributes;

/// <summary>
/// Определяет порядок сериализации свойств
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class SerializationOrderAttribute : Attribute
{
    /// <summary>
    /// Порядок сериализации свойств
    /// </summary>
    public int Order { get; }

    public SerializationOrderAttribute(int order)
    {
        Order = order;
    }
}