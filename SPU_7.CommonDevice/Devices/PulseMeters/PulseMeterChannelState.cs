using System.ComponentModel;

namespace SPU_7.CommonDevice.Devices.PulseMeters;

public enum PulseMeterChannelState : uint
{
    /// <summary>
    /// Состояние канала неизвестно
    /// </summary>
    [Description("Неизвестно")]
    Unknown = 0,
    /// <summary>
    /// Канал в процессе измерения
    /// </summary>
    [Description("Измерение")]
    InProcess = 1,
    /// <summary>
    /// Произошла ошибка
    /// </summary>
    [Description("Ошибка")]
    Error = 2,
    /// <summary>
    /// Не удалось измерить импульс
    /// </summary>
    [Description("Таймаут")]
    Timeout = 4,
    /// <summary>
    /// Измерение завершено
    /// </summary>
    [Description("Завершено")]
    Done = 8,
}