using SPU_7.Modbus.Attributes;

namespace SPU_7.Modbus.Responses;

/// <summary>
/// Ответ на команду записи регистров (<b>0x10<b/>)
/// </summary>
public class WriteMultipleRegistersResponse : BaseResponse
{
    /// <summary>
    /// Начальный адрес регистров
    /// </summary>
    [SerializationOrder(2)]
    public ushort StartingAddress { get; set; }

    /// <summary>
    /// Количество регистров
    /// </summary>
    [SerializationOrder(3)]
    public ushort QuantityOfRegisters { get; set; }
}