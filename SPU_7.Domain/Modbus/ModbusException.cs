using SPU_7.Modbus.Requests;

namespace SPU_7.Domain.Modbus;

public class ModbusException : ApplicationException
{
    public ModbusException()
    {

    }

    public ModbusException(BaseRequest request)
    {
        Request = request;
    }

    public BaseRequest Request { get; }
}