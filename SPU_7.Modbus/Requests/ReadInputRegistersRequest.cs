using SPU_7.Modbus.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Modbus.Requests;

/// <summary>
/// Команда чтения регистров (<b>0x04</b>)
/// </summary>
public class ReadInputRegistersRequest : BaseRequest
{
    /// <param name="deviceAddress">Адрес устройства</param>
    /// <param name="startingAddress">Начальный адрес регистров</param>
    /// <param name="quantityOfRegisters">Количество регистров</param>
    public ReadInputRegistersRequest(byte deviceAddress, ushort startingAddress, ushort quantityOfRegisters)
    {
        DeviceAddress = deviceAddress;
        FunctiomalCode = (byte)FunctiomalCodeEnum.ReadInputRegisters;
        StartingAddress = startingAddress;
        QuantityOfRegisters = quantityOfRegisters;
    }

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

    /// <summary>
    /// размер ответа
    /// </summary>
    /// <returns></returns>
    public override int GetResponseSize()
    {
        return 3 + QuantityOfRegisters * 2;
    }
}