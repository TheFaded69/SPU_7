using System.Buffers.Binary;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.DeviceCommunication.Modbus.Request;

public class WriteMultipleRegistersRequest : ModbusBaseRequest
{
    public WriteMultipleRegistersRequest(byte unitId, ushort startingAddress, ushort registerCount, ArraySegment<byte> registerData, DeviceEndianess endianess)
        : base(unitId, (byte)ModbusFunction.WriteMultipleRegisters, startingAddress, endianess)
    {
        if ((registerCount * 2) != registerData.Count) throw new ArgumentException("Размер данных не совпадает с размером заданного количества регистров.", nameof(registerData));
        RegisterCount = registerCount;
        _registerData = registerData;
    }

    /// <summary>
    /// Сохранённые данные запроса
    /// </summary>
    private byte[]? _requestData;

    /// <summary>
    /// Сохранённые данные регистров
    /// </summary>
    private readonly ArraySegment<byte> _registerData;

    /// <summary>
    /// Создание данных для отправки запроса
    /// </summary>
    private byte[] CreateRequestData()
    {
        var data = new byte[9 + RegisterCount * 2];
        var dseg = new ArraySegment<byte>(data);
        dseg[0] = Address;
        dseg[1] = Function;
        dseg[6] = (byte)_registerData.Count;
        _registerData.CopyTo(data, 7);
        BinaryPrimitives.WriteUInt16BigEndian(dseg.Slice(2, sizeof(ushort)), StartingAddress);
        BinaryPrimitives.WriteUInt16BigEndian(dseg.Slice(4, sizeof(ushort)), RegisterCount);
        BinaryPrimitives.WriteUInt16LittleEndian(dseg.Slice(dseg.Count - sizeof(ushort), sizeof(ushort)), dseg[..^sizeof(ushort)].Crc16());
        return data;
    }

    /// <summary>
    /// Количество регистров
    /// </summary>
    public ushort RegisterCount { get; }

    /// <summary>
    /// Данные для отправки запроса
    /// </summary>
    public override byte[] Data => _requestData ??= CreateRequestData();

    /// <summary>
    /// Ожидаемый размер ответа
    /// </summary>
    public override int ResponseSize => 8;
}