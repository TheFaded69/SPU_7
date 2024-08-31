using SPU_7.Common.Modbus;
using SPU_7.Domain.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Devices.StandDevices.THMeter;

public enum IVTMSensorRegisterMap
{
    [RegisterSetup(0x0002, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2,
        RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    HumidityRegister,

    [RegisterSetup(0x0000, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2,
        RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    TemperatureRegister,
}