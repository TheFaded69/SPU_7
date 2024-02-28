using SPU_7.Modbus.Attributes;
using SPU_7.Modbus.Requests;

namespace SPU_7.Modbus.Responses;

/// <summary>
/// Ответ на команду чтения регистров (<b>0x04</b>)
/// </summary>
public class ReadInputRegistersResponse : BaseResponse
{
    /// <param name="registerQuantity">Количество регистров для чтения</param>
    public ReadInputRegistersResponse(int registerQuantity)
    {
        RegisterValues = new byte[registerQuantity * 2];
    }

    /// <summary>
    /// количество байт (<b>2 * <see cref="ReadInputRegistersRequest.QuantityOfRegisters"/></b>)
    /// </summary>
    [SerializationOrder(2)]
    public byte ByteCount { get; set; }

    /// <summary>
    /// значения регистров
    /// </summary>
    [SerializationOrder(3)]
    public byte[] RegisterValues { get; }
}