using System.Buffers.Binary;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.DeviceCommunication.Modbus.Request;

public class ReadRegistersRequest : ModbusBaseRequest
{
    public ReadRegistersRequest(byte unitId, ushort startingAddress, ushort registerCount, byte function, DeviceEndianess endianess)
        : base(unitId, function, startingAddress, endianess)
    {
        RegisterCount = registerCount;
    }

    /// <summary>
    /// Сохранённые данные запроса
    /// </summary>
    private byte[]? _requestData;

    /// <summary>
    /// Создание данных для отправки запроса
    /// </summary>
    private byte[] CreateRequestData()
    {
        var data = new byte[8];
        var dseg = new ArraySegment<byte>(data);
        dseg[0] = Address;
        dseg[1] = Function;
        BinaryPrimitives.WriteUInt16BigEndian(dseg.Slice(2, sizeof(ushort)), StartingAddress);
        BinaryPrimitives.WriteUInt16BigEndian(dseg.Slice(4, sizeof(ushort)), RegisterCount);
        BinaryPrimitives.WriteUInt16LittleEndian(dseg.Slice(6, sizeof(ushort)), dseg[..^sizeof(ushort)].Crc16());
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
    public override int ResponseSize => 3 + RegisterCount * 2 + sizeof(ushort);
}
