namespace SPU_7.Common.Modbus;

public enum ModbusFunction
{
    ReadHoldingRegisters = 0x03,
    ReadInputRegisters = 0x04,
    WriteSingleRegister = 0x06,
    WriteMultipleRegisters = 0x10
}