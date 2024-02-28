using SPU_7.Modbus.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Modbus.Requests;

/// <summary>
/// Команда записи регистра (<b>0x06</b>)
/// </summary>
public class WriteSingleRegisterRequest : BaseRequest
{
    /// <param name="deviceAddress">Адрес устройства</param>
    /// <param name="address">Адрес регистра</param>
    /// <param name="value">значение для записи (2 байта)</param>
    public WriteSingleRegisterRequest(byte deviceAddress, ushort address, byte[] value)
    {
        DeviceAddress = deviceAddress;
        FunctiomalCode = (byte)FunctiomalCodeEnum.WriteSingleRegister;
        Address = address;
        Value = value;
    }

    /// <summary>
    /// Адрес регистра
    /// </summary>
    [SerializationOrder(2)]
    public ushort Address { get; set; }

    /// <summary>
    /// массив байт для записи (2 байта)
    /// </summary>
    [SerializationOrder(3)]
    public byte[] Value { get; set; }

    /// <summary>
    /// размер ответа
    /// </summary>
    /// <returns></returns>
    public override int GetResponseSize()
    {
        return 6;
    }
}