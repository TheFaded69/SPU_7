using SPU_7.DeviceCommunication.Modbus.Attributes;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.TDIOModules;
public enum KPK_AIN_RegisterMap
{
    /// <summary>
    /// Прочитать блок средних значений аналогового входа
    /// </summary>
    [RegisterConfiguration(0x5000, 10, RegisterDataType.FloatArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    AnalogInputAvgBlock,

    /// <summary>
    /// Среднее значение на аналоговом входе №1
    /// </summary>
    [RegisterConfiguration(0x5000, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    AnalogInput1Avg,

    /// <summary>
    /// Среднее значение на аналоговом входе №2
    /// </summary>
    [RegisterConfiguration(0x5002, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    AnalogInput2Avg,

    /// <summary>
    /// Среднее значение на аналоговом входе №3
    /// </summary>
    [RegisterConfiguration(0x5004, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    AnalogInput3Avg,

    /// <summary>
    /// Среднее значение на аналоговом входе №4
    /// </summary>
    [RegisterConfiguration(0x5006, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    AnalogInput4Avg,

    /// <summary>
    /// Среднее значение на аналоговом входе №5
    /// </summary>
    [RegisterConfiguration(0x5008, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    AnalogInput5Avg,

    /// <summary>
    /// Прочитать блок мгновенных значений аналогового входа
    /// </summary>
    [RegisterConfiguration(0x500A, 10, RegisterDataType.FloatArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    AnalogInputBlock,

    /// <summary>
    /// Значение на аналоговом входе №1
    /// </summary>
    [RegisterConfiguration(0x500A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    AnalogInput1,

    /// <summary>
    /// Значение на аналоговом входе №2
    /// </summary>
    [RegisterConfiguration(0x500C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    AnalogInput2,

    /// <summary>
    /// Значение на аналоговом входе №3
    /// </summary>
    [RegisterConfiguration(0x500E, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    AnalogInput3,

    /// <summary>
    /// Значение на аналоговом входе №4
    /// </summary>
    [RegisterConfiguration(0x5010, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    AnalogInput4,

    /// <summary>
    /// Значение на аналоговом входе №5
    /// </summary>
    [RegisterConfiguration(0x5012, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    AnalogInput5,

    /// <summary>
    /// Маска аналоговых входов
    /// </summary>
    [RegisterConfiguration(0x5018, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    AnalogInputMask,

    /// <summary>
    /// Флаг состояния аналоговых входов
    /// </summary>
    [RegisterConfiguration(0x5019, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    AnalogInputsState
}