using SPU_7.Common.Modbus;
using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Attributes;

/// <summary>
/// Установки регистра
/// </summary>
public class RegisterSetupAttribute : Attribute
{
    public RegisterConfiguration RegisterConfiguration { get; }

    public RegisterSetupAttribute(ushort address, ModbusFunction readFunction, ModbusFunction writeFunction,
        byte registerQuantity, RegisterDataType registerDataType, ByteOrderType byteOrderType)
    {
        RegisterConfiguration = new RegisterConfiguration(address, readFunction, writeFunction, registerQuantity, registerDataType, byteOrderType);
    }
}