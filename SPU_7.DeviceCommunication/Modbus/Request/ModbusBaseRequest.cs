using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.DeviceCommunication.Modbus.Request;

public abstract class ModbusBaseRequest
{
    public ModbusBaseRequest(byte address, byte function, ushort startingAddress, DeviceEndianess endianess)
    {
        Address = address;
        Function = function;
        Endianess = endianess;
		StartingAddress = startingAddress;
    }

    /// <summary>
    /// Адрес в сети Модбас
    /// </summary>
    public byte Address { get; }

    /// <summary>
    /// Код функции
    /// </summary>
    public byte Function { get; }

    /// <summary>
    /// Начальный адрес запроса
    /// </summary>
    public ushort StartingAddress { get; }

    /// <summary>
    /// 
    /// </summary>
    public DeviceEndianess Endianess { get; }

    /// <summary>
    /// Ожидаемый минимальный размер ответа в запросе
    /// </summary>
    public abstract int ResponseSize { get; }

    /// <summary>
    /// Данные для отправки запроса
    /// </summary>
    public abstract byte[] Data { get; }
}