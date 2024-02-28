using SPU_7.Modbus.Requests;

namespace SPU_7.Domain.Modbus;

public class ModbusCrcException : ModbusException
{
    public ModbusCrcException()
    {
    }

    public ModbusCrcException(BaseRequest request) : base(request)
    {
    }
}