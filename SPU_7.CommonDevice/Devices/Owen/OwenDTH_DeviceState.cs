using System.ComponentModel;

namespace SPU_7.CommonDevice.Devices.Owen;

public enum OwenDTH_DeviceState
{
    /// <summary>
    /// Ошибка АЦП - внутренняя ошбка прибора
    /// </summary>
    [Description("Ошибка АЦП")]
    ADC_Error = 0x01,

    /// <summary>
    /// Ошибка датчика холодного спая - внутренняя ошбка прибора, либо выход температуры окружающего воздуха за допустимые пределы
    /// </summary>
    [Description("Ошибка датчика")]
    SensorFault = 0x02,

    /// <summary>
    /// Обрыв ТП - Детектирован обрыв входных цепей сенсора ТП
    /// </summary>
    [Description("Обрыв термопары")]
    NoCircuit = 0x04,

    /// <summary>
    /// Выход за диапазон измерения сопротивления
    /// </summary>
    [Description("R Изм. выход за диапазон")]
    ResistanceOutOfRange = 0x08,

    /// <summary>
    /// Выход за диапазон измерения напряжения

    /// </summary>
    [Description("V Изм. выход за диапазон")]
    VoltageOutOfRange = 0x10,

    /// <summary>
    /// Выход за диапазон измерения температуры
    /// </summary>
    [Description("T Изм. выход за диапазон")]
    MeasuredTemperatureOutOfRange = 0x20,

    /// <summary>
    /// Выход за диапазон регистрации температуры
    /// </summary>
    [Description("T Выход за диапазон")]
    TemperatureOutOfRange = 0x40,

    /// <summary>
    /// Ошибка встроенного ПО
    /// </summary>
    [Description("Ошибка встроенного ПО")]
    FirmwareError = 0x80,
}