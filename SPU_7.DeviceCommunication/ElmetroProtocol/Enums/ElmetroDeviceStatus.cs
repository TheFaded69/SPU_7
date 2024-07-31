using System.ComponentModel;

namespace SPU_7.DeviceCommunication.ElmetroProtocol.Enums;
public enum ElmetroDeviceStatus : byte
{
    /// <summary>
    /// Неизвестное состояние
    /// </summary>
    [Description("Неизвестное состояние")]
    Unknown = 0x00,

    /// <summary>
    /// Получено новое значение давления
    /// </summary>
    [Description("Получено новое значение давления")]
    NewPressureReady = 0x01,

    /// <summary>
    /// Получен новый код АЦП канала температуры
    /// </summary>
    [Description("Получен новый код АЦП канала температуры")]
    NewTemperatureCodeReady = 0x02,

    /// <summary>
    /// Получен новый код АЦП канала давления
    /// </summary>
    [Description("Получен новый код АЦП канала давления")]
    NewPressureCodeReady = 0x04,

    /// <summary>
    /// Недостоверное значение давления
    /// </summary>
    [Description("Недостоверное значение давления")]
    InvalidPressure = 0x08,

    /// <summary>
    /// Ошибочный код АЦП канала температуры
    /// </summary>
    [Description("Ошибочный код АЦП канала температуры")]
    TemperatureCodeError = 0x10,

    /// <summary>
    /// Ошибочный код АЦП канала давления
    /// </summary>
    [Description("Ошибочный код АЦП канала давления")]
    PressureCodeError = 0x20,

    /// <summary>
    /// "Холодный старт" (Первое значение давления после старта устройства ещё не рассчитано)
    /// </summary>
    [Description("Холодный старт")]
    FirstCycle = 0x40,

    /// <summary>
    /// Неисправность устройства (Ошибка АЦП, КС параметров в энергонезависимой памяти и др.).
    /// Дополнительная информация о неисправности может быть получена путём выполнения команды №48 "Чтение расширенного статуса модуля"
    /// </summary>
    [Description("Неисправность устройства")]
    DeviceMalfunction = 0x80,
}
