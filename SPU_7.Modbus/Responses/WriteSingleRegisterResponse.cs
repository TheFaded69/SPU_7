using SPU_7.Modbus.Attributes;

namespace SPU_7.Modbus.Responses;

/// <summary>
/// Ответ на команду записи регистра (<b>0x06<b/>)
/// </summary>
public class WriteSingleRegisterResponse : BaseResponse
{
    /// <summary>
    /// Адрес регистра
    /// </summary>
    [SerializationOrder(2)]
    public ushort Address { get; set; }

    /// <summary>
    /// Значение регистра
    /// </summary>
    [SerializationOrder(3)]
    public byte[] Value { get; set; }
}