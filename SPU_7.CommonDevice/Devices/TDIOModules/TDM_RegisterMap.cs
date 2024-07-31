using SPU_7.DeviceCommunication.Modbus.Attributes;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.TDIOModules;
public enum TDM_RegisterMap
{
    /*
     * Параметры
     */
    /// <summary>
    /// Идентификатор устройства в сети Modbus
    /// </summary>
    [RegisterConfiguration(0x1002, 2, RegisterDataType.UInt32)]
    UnitIdentifier,
    /// <summary>
    /// Номер устройства (Строка)
    /// </summary>
    [RegisterConfiguration(0x1006, 10, RegisterDataType.CharArray)]
    DeviceNumber,
    /// <summary>
    /// Терминальный резистор
    /// </summary>
    [RegisterConfiguration(0x1011, 1, RegisterDataType.UInt16)]
    Resistor,
    /// <summary>
    /// Состояние входов
    /// </summary>
    [RegisterConfiguration(0x1010, 1, RegisterDataType.UInt16, ModbusFunction.ReadInputRegisters, ModbusFunction.None)]
    Inputs,
    /// <summary>
    /// Состояние выходов
    /// </summary>
    [RegisterConfiguration(0x1012, 1, RegisterDataType.UInt16)]
    Outputs,
    /*
     * Параметры порта
     */
    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x2000, 2, RegisterDataType.UInt32)]
    DeviceSpeed,
    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x2002, 2, RegisterDataType.UInt32)]
    StopBits,
    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x2004, 2, RegisterDataType.UInt32)]
    Parity,
    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x2006, 2, RegisterDataType.UInt32)]
    CharactersTimeout,
    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x2008, 2, RegisterDataType.UInt32)]
    RequestTimeout,
    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x200A, 2, RegisterDataType.UInt32)]
    RequestDelay,
    /*
     * Режимы входных каналов
     */
    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x3000, 1, RegisterDataType.UInt16)]
    Channel1Mode,
    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x3001, 1, RegisterDataType.UInt16)]
    Channel2Mode,
    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x3002, 1, RegisterDataType.UInt16)]
    Channel3Mode,
    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x3003, 1, RegisterDataType.UInt16)]
    Channel4Mode,
    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x3004, 1, RegisterDataType.UInt16)]
    Channel5Mode,
    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x3005, 1, RegisterDataType.UInt16)]
    Channel6Mode,
    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x3006, 1, RegisterDataType.UInt16)]
    Channel7Mode,
    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x3007, 1, RegisterDataType.UInt16)]
    Channel8Mode,
    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x3008, 1, RegisterDataType.UInt16)]
    Channel9Mode,
    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x3009, 1, RegisterDataType.UInt16)]
    Channel10Mode,
    /*
     * Физические значения
     */
    [RegisterConfiguration(0x3100, 2, RegisterDataType.UInt32)]
    Input1Value
}