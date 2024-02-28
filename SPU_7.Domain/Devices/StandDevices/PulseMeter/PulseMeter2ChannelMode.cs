namespace SPU_7.Domain.Devices.StandDevices.PulseMeter;

/// <summary>
/// Режим канала
/// </summary>
public enum PulseMeter2ChannelMode
{
    /// <summary>
    /// Выключен
    /// </summary>
    Off,

    /// <summary>
    /// измерение каждый период (по фронту)
    /// </summary>
    MeasureEvery1PeriodFall,

    /// <summary>
    /// измерение каждый период (по фронту)
    /// </summary>
    MeasureEvery1PeriodRise,

    /// <summary>
    /// измерение каждые 4 периода (по фронту)
    /// </summary>
    MeasureEvery4PeriodRise,

    /// <summary>
    /// измерение каждые 16 периодов (по фронту)
    /// </summary>
    MeasureEvery16PeriodRise,
}