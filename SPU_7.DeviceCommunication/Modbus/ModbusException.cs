using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.DeviceCommunication.Modbus;

public class ModbusException : Exception
{
    public ModbusException(ArraySegment<byte> dataFrameReceived, string message) : base(message)
    {
        ExceptionCode -= 1;
        DataFrameReceived = dataFrameReceived;
    }

    public ModbusException(ArraySegment<byte> dataFrameReceived, ModbusExceptionCode exceptionCode, string message) : base(message)
    {
        ExceptionCode = exceptionCode;
        DataFrameReceived = dataFrameReceived;
    }

    /// <summary>
    /// Код исключения Modbus. Значение -1 (0xFF) говорит о том, что нет кода исключения.
    /// </summary>
    public ModbusExceptionCode ExceptionCode { get; }

    /// <summary>
    /// Данные о кадре Modbus которые принял порт
    /// </summary>
    public ArraySegment<byte> DataFrameReceived { get; }
}