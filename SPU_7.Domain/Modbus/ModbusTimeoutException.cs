using SPU_7.Modbus.Requests;

namespace SPU_7.Domain.Modbus;

public class ModbusTimeoutException : ModbusException
{
    public ModbusTimeoutException()
    {
    }

    public ModbusTimeoutException(BaseRequest request) : base(request)
    {
    }
}