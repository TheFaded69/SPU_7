using SPU_7.Common.Modbus;
using SPU_7.Domain.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Devices.StandDevices.PressureSensor;

public enum PressureSensor415MRegisterMap
{
    /// <summary>
    /// Калибровка нуля
    /// </summary>
    [RegisterSetup(0x500A, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    PressureRegister,
    
    /// <summary>
    /// Давление в мм.вод.ст.
    /// </summary>
    [RegisterSetup(0x0054, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    CurrentWaterStickPressure,
}