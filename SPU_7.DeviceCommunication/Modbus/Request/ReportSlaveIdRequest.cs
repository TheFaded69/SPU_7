using System.Buffers.Binary;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.DeviceCommunication.Modbus.Request;
public class ReportSlaveIdRequest : ModbusBaseRequest
{
    public ReportSlaveIdRequest(byte address, DeviceEndianess endianess) : base(address, (byte)ModbusFunction.ReportSlaveId, 0, endianess)
    {
    }

    /// <summary>
    /// Сохранённые данные запроса
    /// </summary>
    private byte[]? _requestData;

    public override int ResponseSize => 5;
    public override byte[] Data => _requestData ??= CreateRequestData();

    /// <summary>
    /// Формирование данных для запроса
    /// </summary>
    private byte[] CreateRequestData()
    {
        var data = new byte[4];
        var dseg = new ArraySegment<byte>(data);
        dseg[0] = Address;
        dseg[1] = Function;
        BinaryPrimitives.WriteUInt16LittleEndian(dseg.Slice(2, sizeof(ushort)), dseg[..^sizeof(ushort)].Crc16());
        return data;
    }
}