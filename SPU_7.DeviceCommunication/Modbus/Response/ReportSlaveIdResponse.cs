using SPU_7.DeviceCommunication.Modbus.Enums;
using SPU_7.DeviceCommunication.Modbus.Request;

namespace SPU_7.DeviceCommunication.Modbus.Response;
public class ReportSlaveIdResponse : ModbusBaseResponse
{
    public ReportSlaveIdResponse(ModbusBaseRequest request, ArraySegment<byte> frameData, DeviceEndianess endianess) : base(request, frameData, endianess)
    {
    }

    public int DataSize => FrameData[2] - 2;

    /// <summary>
    /// Данные, которые прислало устройство по запросу
    /// </summary>
    public ArraySegment<byte> Data => FrameData.Slice(3, FrameData.Count - 5);
}