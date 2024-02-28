using SPU_7.Modbus.Attributes;
using SPU_7.Modbus.Requests;

namespace SPU_7.Modbus.Responses;

/// <summary>
/// Ответ на команду чтения регистров (<b>0x03</b>)
/// </summary>
public class ReadHoldingRegistersResponse : BaseResponse
{
    /// <param name="quantityOfRegistersToRead">Количество регистров для чтения</param>
    public ReadHoldingRegistersResponse(int quantityOfRegistersToRead)
    {
        RegisterValues = new byte[quantityOfRegistersToRead * 2];
    }

    /// <summary>
    /// только для тестирования
    /// </summary>
    /// <param name="registerValues"></param>
    public ReadHoldingRegistersResponse(byte[] registerValues)
    {
        RegisterValues = registerValues;
    }

    /// <summary>
    /// количество байт (<b>2 * <see cref="ReadHoldingRegistersRequest.QuantityOfRegisters"/></b>)
    /// </summary>
    [SerializationOrder(2)]
    public byte ByteCount { get; set; }

    /// <summary>
    /// данные регистров
    /// </summary>
    [SerializationOrder(3)]
    public byte[] RegisterValues { get; }
}