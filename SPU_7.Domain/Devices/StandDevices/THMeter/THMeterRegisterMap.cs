using SPU_7.Common.Modbus;
using SPU_7.Domain.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Devices.StandDevices.THMeter
{
    public enum THMeterRegisterMap
    {
        [RegisterSetup(0x0000, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 1, RegisterDataType.UInt16, ByteOrderType.MidLittleEndian_CDAB)]
        HumidityRegister,

        [RegisterSetup(0x0001, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 1, RegisterDataType.Int16, ByteOrderType.MidLittleEndian_CDAB)]
        TemperatureRegister,
    }
}
