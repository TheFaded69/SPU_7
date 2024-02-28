namespace SPU_7.Domain.Devices.StandDevices.PulseMeter;

/// <summary>
/// Статус канала
/// </summary>
public enum PulseMeter2ChannelState : uint
{
    /// <summary>
    /// Неизвестно
    /// </summary>
    None = 0,

    /// <summary>
    /// В процессе измерения
    /// </summary>
    Running = 1,

    /// <summary>
    /// Ошибка
    /// </summary>
    Error = 2,

    /// <summary>
    /// Таймаут
    /// </summary>
    Timeout = 4,

    /// <summary>
    /// Завершено удачно
    /// </summary>
    Ok = 8
}