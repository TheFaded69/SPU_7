using SPU_7.Modbus.Requests;

namespace SPU_7.Domain.Modbus;

public class ModbusDataTransmissionException : ModbusException
{
    public ModbusDataTransmissionException()
    {
    }

    public ModbusDataTransmissionException(BaseRequest request) : base(request)
    {
    }
}