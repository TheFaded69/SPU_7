using SPU_7.Common.Modbus;
using SPU_7.Domain.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Devices.StandDevices.Owen.OwenPBR10A;

public enum OwenPBR10ARegisterMap
{
    /// <summary>
    /// Положение/Величина закрытия арматуры, в % [0-100]
    /// </summary>
    [RegisterSetup(0x2745, 
        ModbusFunction.ReadHoldingRegisters,
        ModbusFunction.WriteMultipleRegisters, 
        1, 
        RegisterDataType.UInt16, 
        ByteOrderType.MidLittleEndian_CDAB)]
    PositionPercent,
    
    [RegisterSetup(0x2719, 
        ModbusFunction.ReadHoldingRegisters,
        ModbusFunction.WriteMultipleRegisters, 
        1, 
        RegisterDataType.UInt16, 
        ByteOrderType.MidLittleEndian_CDAB)]
    ValveControl,
}