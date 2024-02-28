using SPU_7.Modbus.Attributes;
using SPU_7.Modbus.Requests;

namespace SPU_7.Modbus.Responses;

/// <summary>
/// Ответ на команду записи регистров с последующим чтением регистров (<b>0x17</b>)
/// </summary>
public class ReadWriteMultipleRegistersResponse : BaseResponse
{
    /// <param name="registerQuantity">Количество регистров для чтения</param>
    public ReadWriteMultipleRegistersResponse(int registerQuantity)
    {
        RegisterValues = new byte[registerQuantity * 2];
    }

    /// <summary>
    /// Количество байт (<b>2 * <see cref="ReadWriteMultipleRegistersRequest.QuantityToRead"/></b>)
    /// </summary>
    [SerializationOrder(2)]
    public byte ByteCount { get; set; }

    /// <summary>
    /// значения регистров
    /// </summary>
    [SerializationOrder(3)]
    public byte[] RegisterValues { get; }
}