using System.ComponentModel;

namespace SPU_7.DeviceCommunication.ElmetroProtocol.Enums;

public enum PressureSensorType
{
    /// <summary>
    /// Модуль избыточного давления
    /// </summary>
    [Description("Модуль избыточного давления")]
    PressureType = 0x00,

    /// <summary>
    /// Модуль абсолютного давления
    /// </summary>
    [Description("Модуль абсолютного давления")]
    AbsolutePressureType = 0x01,

    /// <summary>
    /// Модуль вакууметрического давления
    /// </summary>
    [Description("Модуль вакууметрического давления")]
    AbsoluteVacuumPressureType = 0x02,

    /// <summary>
    /// Модуль избыточно-вакууметрического давления
    /// </summary>
    [Description("Модуль избыточно-вакууметрического давления")]
    VacuumPressureType = 0x03,
}