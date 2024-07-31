using SPU_7.DeviceCommunication.Modbus.Attributes;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.PS415M;

/// <summary>
/// Карта регистров для датчика давления 415М, Смещение Адреса регистра указывается в байтах
/// </summary>
public enum PS415M_RegisterMap
{
    /// <summary>
    /// Калибровка нуля
    /// </summary>
    [RegisterConfiguration(0x0030, 1, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, writeFunction: ModbusFunction.None)]
    ZeroCalibration,

    /// <summary>
    /// Калибровка диапазона
    /// </summary>
    [RegisterConfiguration(0x0044, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, writeFunction: ModbusFunction.None)]
    RangeCalibration,

    /// <summary>
    /// Восстановление заводских настроек диапазона
    /// </summary>
    [RegisterConfiguration(0x0050, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, writeFunction: ModbusFunction.None)]
    RestoreRangeCalibration,

    /// <summary>
    /// Текущая температура в °C
    /// </summary>
    [RegisterConfiguration(0x0050, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, writeFunction: ModbusFunction.None)]
    CurrentTemperature,

    /// <summary>
    /// ???
    /// </summary>
    //[RegisterConfiguration(0x0052, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, writeFunction: ModbusFunction.None)]
    //UnknownValue1, // 0.406

    /// <summary>
    /// Давление в мм.вод.ст.
    /// </summary>
    [RegisterConfiguration(0x0054, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, writeFunction: ModbusFunction.None)]
    CurrentWaterStickPressure,

    /// <summary>
    /// ???
    /// </summary>
    //[RegisterConfiguration(0x0056, 2, RegisterDataType.Int32, ModbusFunction.ReadInputRegisters, writeFunction: ModbusFunction.None)]
    //UnknownValue2,

    /// <summary>
    /// Давление в кПа
    /// </summary>
    [RegisterConfiguration(0x0058, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, writeFunction: ModbusFunction.None)]
    CurrentPressure,

    /// <summary>
    /// ???
    /// </summary>
    //[RegisterConfiguration(0x0060, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, writeFunction: ModbusFunction.None)]
    //UnknownValue3, // всегда = 0

    /// <summary>
    /// Заводской номер
    /// </summary>
    [RegisterConfiguration(0x01F8, 1, RegisterDataType.Int16, ModbusFunction.ReadHoldingRegisters, writeFunction: ModbusFunction.None)]
    VendorNumber,

    /// <summary>
    /// Номер модели
    /// </summary>
    [RegisterConfiguration(0x01FA, 1, RegisterDataType.Int16, ModbusFunction.ReadHoldingRegisters, writeFunction: ModbusFunction.None)]
    ModelNumber,
}