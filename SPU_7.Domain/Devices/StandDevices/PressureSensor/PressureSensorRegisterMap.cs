using SPU_7.Common.Modbus;
using SPU_7.Domain.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Devices.StandDevices.PressureSensor;

public enum PressureSensorRegisterMap
{
    [RegisterSetup(0x0000, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    PressureRegister,

    [RegisterSetup(0x0010, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    ResetToZeroRegister,
}