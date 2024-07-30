using SPU_7.Common.Modbus;
using SPU_7.Domain.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Devices.StandDevices.TemperatureSensor;

public enum TemperatureSensorRegisterMap
{
    [RegisterSetup(0x0000, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    TemperatureRegister,
    
    [RegisterSetup(0x5000, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    Channel1Register,
    
    [RegisterSetup(0x5002, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    Channel2Register,
    
    [RegisterSetup(0x5004, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    Channel3Register,
    
    [RegisterSetup(0x5006, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    Channel4Register,
    
    [RegisterSetup(0x5008, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    Channel5Register,

}