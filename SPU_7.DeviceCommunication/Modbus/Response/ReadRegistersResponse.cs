using SPU_7.DeviceCommunication.Modbus.Enums;
using SPU_7.DeviceCommunication.Modbus.Request;

namespace SPU_7.DeviceCommunication.Modbus.Response;

public class ReadRegistersResponse : ModbusBaseResponse
{
    public ReadRegistersResponse(ReadRegistersRequest request, ArraySegment<byte> frameData, DeviceEndianess endianess) : base(request, frameData, endianess)
    {
    }

    /// <summary>
    /// Размер данных в ответе в байтах
    /// </summary>
    public int DataSize => FrameData[2];

    /// <summary>
    /// Данные в кадре
    /// </summary>
    public ArraySegment<byte> Data => FrameData.Slice(3, DataSize > (FrameData.Count - 5) ? FrameData.Count - 5 : DataSize);
}