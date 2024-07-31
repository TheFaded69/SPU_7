using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.DeviceCommunication.Modbus.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class RegisterConfigurationAttribute : Attribute
{
    public RegisterConfigurationAttribute(ushort address, ushort numOfRegisters, RegisterDataType registerDataType,
                    ModbusFunction readFunction = ModbusFunction.ReadInputRegisters, ModbusFunction writeFunction = ModbusFunction.WriteMultipleRegisters)
    {
        RegisterConfiguration = new RegisterConfiguration(address, numOfRegisters, registerDataType, readFunction, writeFunction);
    }

    public RegisterConfiguration RegisterConfiguration { get; }
}