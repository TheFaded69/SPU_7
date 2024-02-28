using SPU_7.Modbus.Requests;

namespace SPU_7.Domain.Modbus;

public class ModbusErrorException : ModbusException
{
    public ModbusErrorException()
    {
    }

    public ModbusErrorException(BaseRequest request, byte exceptionCode) : base(request)
    {
        ExceptionCode = exceptionCode;
    }

    public byte ExceptionCode { get; }
}