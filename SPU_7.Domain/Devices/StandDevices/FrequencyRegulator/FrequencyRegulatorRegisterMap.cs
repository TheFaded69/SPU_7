using System.ComponentModel;
using SPU_7.Common.Modbus;
using SPU_7.Domain.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Devices.StandDevices.FrequencyRegulator;

public enum FrequencyRegulatorRegisterMap
{
    [RegisterSetup(0x2000, 
        ModbusFunction.ReadHoldingRegisters, 
        ModbusFunction.WriteSingleRegister, 
        1, 
        RegisterDataType.UInt16, 
        ByteOrderType.BigEndian_ABCD)]
    CommandRegister,
    
    [RegisterSetup(0x2001, 
        ModbusFunction.ReadHoldingRegisters, 
        ModbusFunction.WriteSingleRegister, 
        1, 
        RegisterDataType.UInt16, 
        ByteOrderType.BigEndian_ABCD)]
    FrequencyValueRegister,

    #region PA param

    [RegisterSetup(0x0002, 
        ModbusFunction.ReadHoldingRegisters, 
        ModbusFunction.WriteMultipleRegisters, 
        1, 
        RegisterDataType.UInt16, 
        ByteOrderType.BigEndian_ABCD)]
    [Description("Выходная частота")]
    PA02,
    
    #endregion
    
    #region Pb param


    
    #endregion
    
    #region PC param

    

    #endregion
    
    #region Pd param

    

    #endregion
    
}