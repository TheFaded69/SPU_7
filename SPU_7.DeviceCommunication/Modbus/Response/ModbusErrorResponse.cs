using SPU_7.DeviceCommunication.Modbus.Enums;
using SPU_7.DeviceCommunication.Modbus.Request;

namespace SPU_7.DeviceCommunication.Modbus.Response;

public class ModbusErrorResponse : ModbusBaseResponse
{
    public ModbusErrorResponse(ModbusBaseRequest request, ArraySegment<byte> frameData, DeviceEndianess endianess) : base(request, frameData, endianess)
    {

    }
}