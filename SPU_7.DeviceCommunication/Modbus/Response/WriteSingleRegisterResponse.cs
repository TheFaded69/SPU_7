using System.Buffers.Binary;
using SPU_7.DeviceCommunication.Modbus.Enums;
using SPU_7.DeviceCommunication.Modbus.Request;

namespace SPU_7.DeviceCommunication.Modbus.Response;

public class WriteSingleRegisterResponse : ModbusBaseResponse
{
    public WriteSingleRegisterResponse(WriteSingleRegisterRequest request, ArraySegment<byte> frameData, DeviceEndianess endianess) : base(request, frameData, endianess)
    {
        var startingAddress = BinaryPrimitives.ReadUInt16BigEndian(frameData.Slice(2, sizeof(ushort)));
        if (request.StartingAddress != startingAddress) throw new ModbusException(frameData, $"Несовпадение адресов регистра! В запросе: {request.StartingAddress}, в ответе: {startingAddress}");
    }

    /// <summary>
    /// 
    /// </summary>
    public ArraySegment<byte> RegisterValue => FrameData.Slice(4, 2);
}