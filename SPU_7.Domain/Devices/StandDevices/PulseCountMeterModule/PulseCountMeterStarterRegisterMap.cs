using SPU_7.Common.Modbus;
using SPU_7.Domain.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Devices.StandDevices.PulseCountMeterModule;

public enum PulseCountMeterStarterRegisterMap
{
    [RegisterSetup(0x001E,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.UInt32,
        ByteOrderType.MidLittleEndian_CDAB)]
    CommandRegister,
}