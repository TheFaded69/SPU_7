using SPU_7.Common.Modbus;
using SPU_7.Domain.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Devices.StandDevices.NeedleValveController;

public enum NeedleValveControllerRegisterMap
{
    [RegisterSetup(0x0012, 
        ModbusFunction.ReadHoldingRegisters,
        ModbusFunction.WriteMultipleRegisters, 
        2, 
        RegisterDataType.UInt32, 
        ByteOrderType.MidLittleEndian_CDAB)]
    StatusRegister,
    
    [RegisterSetup(0x0014, 
        ModbusFunction.ReadHoldingRegisters,
        ModbusFunction.WriteMultipleRegisters, 
        2, 
        RegisterDataType.UInt32, 
        ByteOrderType.MidLittleEndian_CDAB)]
    CommandRegister,
    
    [RegisterSetup(0x0016, 
        ModbusFunction.ReadHoldingRegisters,
        ModbusFunction.WriteMultipleRegisters, 
        2, 
        RegisterDataType.UInt32, 
        ByteOrderType.MidLittleEndian_CDAB)]
    ParameterRegister,
    
    [RegisterSetup(0x0018, 
        ModbusFunction.ReadHoldingRegisters,
        ModbusFunction.WriteMultipleRegisters, 
        2, 
        RegisterDataType.UInt32, 
        ByteOrderType.MidLittleEndian_CDAB)]
    CurrentParameterRegister,
    
}