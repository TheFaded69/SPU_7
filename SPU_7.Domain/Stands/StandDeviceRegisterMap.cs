using SPU_7.Common.Modbus;
using SPU_7.Domain.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Stands;

public enum StandDeviceRegisterMap
{
    /// <summary>
    /// DOut address
    /// </summary>
    [RegisterSetup(0x1012, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 1, RegisterDataType.UInt16, ByteOrderType.BigEndian_ABCD)]
    ControlRegister,

    /// <summary>
    /// DIn address
    /// </summary>
    [RegisterSetup(0x1010, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 1, RegisterDataType.UInt16, ByteOrderType.BigEndian_ABCD)]
    InfoRegister,
}