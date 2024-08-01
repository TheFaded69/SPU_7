using SPU_7.Common.Modbus;
using SPU_7.Domain.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Devices.StandDevices.PulseCountMeterModule;

public enum PulseMeterCountModuleRegisterMap
{
    #region Input register

    /// <summary>
    /// Регистр широковещательой команды
    /// </summary>
    [RegisterSetup(0x0034,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.UInt32,
        ByteOrderType.MidLittleEndian_CDAB)]
    CommonCommandStatusRegister,

    [RegisterSetup(0x0000,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    Register1,

    [RegisterSetup(0x0002,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    Register2,

    [RegisterSetup(0x0004,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    Register3,

    [RegisterSetup(0x0006,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    Register4,

    [RegisterSetup(0x0008,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    Register5,

    [RegisterSetup(0x000A,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    Register6,

    [RegisterSetup(0x000C,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    Register7,

    [RegisterSetup(0x000E,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    Register8,

    [RegisterSetup(0x0010,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    PulseDurationRegisterChannel1,

    [RegisterSetup(0x0012,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    PulsePeriodRegisterChannel1,
    
    [RegisterSetup(0x0014,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    StatusRegisterChannel1,
    
    
    [RegisterSetup(0x0018,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    PulseDurationRegisterChannel2,
    
    [RegisterSetup(0x001A,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    PulsePeriodRegisterChannel2,
    
    [RegisterSetup(0x001C,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    StatusRegisterChannel2,
    
    
    [RegisterSetup(0x0020,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    PulseDurationRegisterChannel3,
    
    [RegisterSetup(0x0022,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    PulsePeriodRegisterChannel3,
    
    [RegisterSetup(0x0024,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    StatusRegisterChannel3,
    
    [RegisterSetup(0x0028,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    PulseDurationRegisterChannel4,
    
    [RegisterSetup(0x002A,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    PulsePeriodRegisterChannel4,
    
    [RegisterSetup(0x002C,
        ModbusFunction.ReadInputRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.Float,
        ByteOrderType.MidLittleEndian_CDAB)]
    StatusRegisterChannel4,

    #endregion


    #region Holding register

    /// <summary>
    /// Регистр широковещательой команды
    /// </summary>
    [RegisterSetup(0x001E,
        ModbusFunction.ReadHoldingRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.UInt32,
        ByteOrderType.MidLittleEndian_CDAB)]
    CommonCommandRegister,

    /// <summary>
    /// Регистр выбора профиля настроек
    /// </summary>
    [RegisterSetup(0x001C,
        ModbusFunction.ReadHoldingRegisters,
        ModbusFunction.WriteMultipleRegisters,
        2,
        RegisterDataType.UInt32,
        ByteOrderType.MidLittleEndian_CDAB)]
    SettingsProfileRegister,
    
    #endregion
}