using System.Buffers.Binary;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.DeviceCommunication.Modbus.Request;

public class WriteSingleRegisterRequest : ModbusBaseRequest
{
    public WriteSingleRegisterRequest(byte unitId, ushort startingAddress, ArraySegment<byte> registerValue, DeviceEndianess endianess)
        : base(unitId, (byte)ModbusFunction.WriteSingleRegister, startingAddress, endianess)
    {
        _registerValue = registerValue;
    }

    /// <summary>
    /// Сохранённые данные запроса
    /// </summary>
    private byte[]? _requestData;

    /// <summary>
    /// Значение регистра
    /// </summary>
    private readonly ArraySegment<byte> _registerValue;

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
        dseg[4] = _registerValue[0];
        dseg[5] = _registerValue[1];
        BinaryPrimitives.WriteUInt16LittleEndian(dseg.Slice(6, sizeof(ushort)), dseg[..^sizeof(ushort)].Crc16());
        return data;
    }

    /// <summary>
    /// Данные для отправки запроса
    /// </summary>
    public override byte[] Data => _requestData ??= CreateRequestData();

    /// <summary>
    /// Ожидаемый размер ответа
    /// </summary>
    public override int ResponseSize => 8;
}