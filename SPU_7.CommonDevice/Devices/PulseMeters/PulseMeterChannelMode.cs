using System.ComponentModel;

namespace SPU_7.CommonDevice.Devices.PulseMeters;

public enum PulseMeterChannelMode
{
    /// <summary>
    /// Канал отключен
    /// </summary>
    [Description("Отключен")]
    Off,
    /// <summary>
    /// Считать один импульс за одно падение
    /// </summary>
    [Description("При падении")]
    Fall,
    /// <summary>
    /// Считать один импульс за одно возрастание
    /// </summary>
    [Description("При возрастании")]
    Rise,
    /// <summary>
    /// Считать один импульс за 4 возрастания
    /// </summary>
    [Description("Каждые 4 возрастания")]
    Every4Rise,
    /// <summary>
    /// Считать один импульс за 16 возрастаний
    /// </summary>
    [Description("Каждые 16 возрастаний")]
    Every16Rise,
}