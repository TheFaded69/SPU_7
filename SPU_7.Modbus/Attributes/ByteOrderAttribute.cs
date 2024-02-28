using SPU_7.Modbus.Types;

namespace SPU_7.Modbus.Attributes;

/// <summary>
/// Определяет порядок следования байт
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ByteOrderAttribute : Attribute
{
    /// <summary>
    /// Порядок следования байт
    /// </summary>
    public ByteOrderType Order { get; }

    public ByteOrderAttribute(ByteOrderType byteOrder)
    {
        Order = byteOrder;
    }
}